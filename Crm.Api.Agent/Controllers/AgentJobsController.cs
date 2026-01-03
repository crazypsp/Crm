using Crm.Api.Agent.Contracts;
using Crm.Data;
using Crm.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Agent.Controllers;

[ApiController]
[Route("api/agent/jobs")]
[Authorize] // Neden: Agent job API dışarıya açık kalmamalı
public sealed class AgentJobsController : ControllerBase
{
    private readonly CrmDbContext _db;
    private readonly IConfiguration _cfg;

    public AgentJobsController(CrmDbContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    // Enum isimleri projede farklı olabilir; compile garantisi için numerik sabit kullanıyoruz.
    // Eğer senin enum sıralaman farklıysa, aşağıdaki sayıları senin enum değerlerine göre güncelle.
    private static readonly IntegrationJobStatus Queued = (IntegrationJobStatus)0;
    private static readonly IntegrationJobStatus InProgress = (IntegrationJobStatus)1;
    private static readonly IntegrationJobStatus Completed = (IntegrationJobStatus)2;
    private static readonly IntegrationJobStatus Failed = (IntegrationJobStatus)3;
    private static readonly IntegrationJobStatus DeadLetter = (IntegrationJobStatus)4;

    /// <summary>
    /// Agent "sıradaki işi" ister.
    /// Neden: İşin iki farklı agent tarafından aynı anda alınmasını lock ile engelleriz.
    /// </summary>
    [HttpGet("next")]
    public async Task<ActionResult<NextJobResponse>> Next(
        [FromQuery] Guid tenantId,
        [FromQuery] string agentId,
        [FromQuery] Guid? agentMachineId,
        [FromQuery] Guid? companyId,
        CancellationToken ct)
    {
        if (tenantId == Guid.Empty)
            return BadRequest(new { message = "tenantId zorunludur." });

        if (string.IsNullOrWhiteSpace(agentId))
            return BadRequest(new { message = "agentId zorunludur." });

        var lockMinutes = _cfg.GetValue<int>("Agent:LockMinutes", 10);
        var maxAttempts = _cfg.GetValue<int>("Agent:MaxAttempts", 5);

        var now = DateTimeOffset.UtcNow;
        var lockUntil = now.AddMinutes(lockMinutes);

        // Neden: SQL Server’da SKIP LOCKED yok; bu nedenle transaction içinde seç+lock+save yapıyoruz.
        await using var trx = await _db.Database.BeginTransactionAsync(ct);

        // 1) Uygun job’ı seç
        // - Queued olan
        // - veya InProgress olup lock süresi geçmiş olan (agent öldü/kapandı senaryosu)
        // - DeadLetter olmayan
        // - Attempts < maxAttempts
        var q = _db.IntegrationJobs
            .Where(j => j.TenantId == tenantId && !j.IsDeleted)
            .Where(j => j.Status != DeadLetter)
            .Where(j => j.Attempts < maxAttempts)
            .Where(j =>
                j.Status == Queued ||
                (j.Status == InProgress && j.LockedUntil != null && j.LockedUntil < now));

        // Opsiyonel filtreler (MVP):
        // - Eğer agentMachineId verilirse pinned job’ları önceliklendiririz.
        if (agentMachineId is not null)
        {
            q = q.Where(j => j.AgentMachineId == null || j.AgentMachineId == agentMachineId);
        }

        // - Eğer companyId verilirse company scope’ta job döndürürüz (multi-company agent senaryosu).
        if (companyId is not null)
        {
            q = q.Where(j => j.CompanyId == null || j.CompanyId == companyId);
        }

        var job = await q
            .OrderBy(j => j.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (job is null)
        {
            await trx.CommitAsync(ct);
            return NoContent();
        }

        // 2) Lock + attempt increment (atomic)
        job.Status = InProgress;
        job.LockedBy = agentId;
        job.LockedUntil = lockUntil;
        job.Attempts += 1;
        job.UpdatedAt = now;

        await _db.SaveChangesAsync(ct);
        await trx.CommitAsync(ct);

        // 3) Agent’a geri dön
        return Ok(new NextJobResponse
        {
            JobId = job.Id,
            JobType = job.CommandType,        // DTO alan adı JobType ama içerik CommandType
            PayloadJson = job.PayloadJson ?? "{}",
            LockedUntil = job.LockedUntil!.Value,
            Attempts = job.Attempts
        });
    }

    /// <summary>
    /// Agent iş başarılı bitirdi.
    /// Neden: job Completed olur, lock kaldırılır, result yazılır.
    /// </summary>
    [HttpPost("{jobId:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid jobId,
        [FromQuery] Guid tenantId,
        [FromQuery] string agentId,
        [FromBody] CompleteJobRequest req,
        CancellationToken ct)
    {
        if (tenantId == Guid.Empty) return BadRequest(new { message = "tenantId zorunludur." });
        if (string.IsNullOrWhiteSpace(agentId)) return BadRequest(new { message = "agentId zorunludur." });

        var job = await _db.IntegrationJobs
            .FirstOrDefaultAsync(j => j.Id == jobId && j.TenantId == tenantId && !j.IsDeleted, ct);

        if (job is null) return NotFound();

        // Neden: Sadece lock sahibi agent complete edebilsin (double complete engeli)
        if (!string.Equals(job.LockedBy, agentId, StringComparison.OrdinalIgnoreCase))
            return Forbid();

        job.Status = Completed;
        job.LockedBy = null;
        job.LockedUntil = null;
        job.LastError = null;
        job.ResultJson = req.ResultJson;
        job.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(ct);
        return Ok(new { ok = true });
    }

    /// <summary>
    /// Agent işte hata aldı.
    /// Neden: Server retry veya dead-letter kararını verir.
    /// </summary>
    [HttpPost("{jobId:guid}/fail")]
    public async Task<IActionResult> Fail(
        Guid jobId,
        [FromQuery] Guid tenantId,
        [FromQuery] string agentId,
        [FromBody] FailJobRequest req,
        CancellationToken ct)
    {
        if (tenantId == Guid.Empty) return BadRequest(new { message = "tenantId zorunludur." });
        if (string.IsNullOrWhiteSpace(agentId)) return BadRequest(new { message = "agentId zorunludur." });
        if (string.IsNullOrWhiteSpace(req.ErrorMessage)) return BadRequest(new { message = "ErrorMessage zorunludur." });

        var maxAttempts = _cfg.GetValue<int>("Agent:MaxAttempts", 5);

        var job = await _db.IntegrationJobs
            .FirstOrDefaultAsync(j => j.Id == jobId && j.TenantId == tenantId && !j.IsDeleted, ct);

        if (job is null) return NotFound();

        if (!string.Equals(job.LockedBy, agentId, StringComparison.OrdinalIgnoreCase))
            return Forbid();

        job.LastError = req.ErrorMessage.Length > 2000 ? req.ErrorMessage[..2000] : req.ErrorMessage;
        job.LockedBy = null;
        job.LockedUntil = null;
        job.UpdatedAt = DateTimeOffset.UtcNow;

        // Attempts, Next() sırasında artırıldı. Fail sadece statü kararını verir.
        if (job.Attempts >= maxAttempts)
        {
            job.Status = DeadLetter; // manuel müdahale
        }
        else
        {
            // Retry: tekrar kuyruğa al
            job.Status = Failed; // log/rapor için
            job.Status = Queued; // kuyruğa geri
        }

        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            ok = true,
            status = (int)job.Status,
            attempts = job.Attempts
        });
    }

    /// <summary>
    /// UI/Support: job detayını gör.
    /// </summary>
    [HttpGet("{jobId:guid}")]
    public async Task<IActionResult> Get(
        Guid jobId,
        [FromQuery] Guid tenantId,
        CancellationToken ct)
    {
        if (tenantId == Guid.Empty) return BadRequest(new { message = "tenantId zorunludur." });

        var job = await _db.IntegrationJobs.AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == jobId && j.TenantId == tenantId && !j.IsDeleted, ct);

        if (job is null) return NotFound();

        return Ok(new
        {
            job.Id,
            job.TenantId,
            job.CompanyId,
            job.AgentMachineId,
            job.CommandType,
            payloadJson = job.PayloadJson,
            resultJson = job.ResultJson,
            status = (int)job.Status,
            job.Attempts,
            job.LockedBy,
            job.LockedUntil,
            job.LastError,
            job.CreatedAt,
            job.UpdatedAt
        });
    }
}
