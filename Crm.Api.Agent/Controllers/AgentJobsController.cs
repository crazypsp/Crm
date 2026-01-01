using System.Data;
using Crm.Data;
using Crm.Entities.Enums;
using Crm.Entities.Integration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Agent.Controllers
{
    [ApiController]
    [Route("api/agent/jobs")]
    public sealed class AgentJobsController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public AgentJobsController(CrmDbContext db)
        {
            _db = db;
        }

        [HttpGet("next")]
        public async Task<IActionResult> Next([FromQuery] Guid companyId, CancellationToken ct)
        {
            // Neden: Agent, belirli bir şirkete (mükellef) bağlı işleri çekebilir.
            // İstersen ileride "companyId zorunlu olmasın" yapar, agent’a yetkili şirketleri DB’den veririz.

            var agentMachineId = (Guid)HttpContext.Items["AgentMachineId"]!;
            var tenantId = (Guid)HttpContext.Items["TenantId"]!;

            // 1) Atomik claim: yarış koşullarını engellemek için tek SQL ile.
            var jobId = await ClaimNextJobIdAsync(tenantId, companyId, agentMachineId, ct);

            if (jobId is null)
                return NoContent();

            // 2) Job’ı okuyup payload döndür
            // TODO: alan adları sende farklıysa uyarlayacaksın.
            var job = await _db.IntegrationJobs.AsNoTracking()
                .Where(x => x.Id == jobId.Value)
                .Select(x => new AgentJobDto
                {
                    JobId = x.Id,
                    CommandType = x.CommandType,
                    PayloadJson = x.PayloadJson
                })
                .FirstAsync(ct);

            return Ok(job);
        }

        [HttpPost("{jobId:guid}/complete")]
        public async Task<IActionResult> Complete(Guid jobId, [FromBody] CompleteJobRequest req, CancellationToken ct)
        {
            var agentMachineId = (Guid)HttpContext.Items["AgentMachineId"]!;
            var tenantId = (Guid)HttpContext.Items["TenantId"]!;

            // Neden: Job sadece onu claim eden agent tarafından tamamlanabilmeli (ownership).
            var job = await _db.IntegrationJobs.FirstOrDefaultAsync(x =>
                x.Id == jobId &&
                x.TenantId == tenantId &&
                x.AgentMachineId == agentMachineId &&
                !x.IsDeleted, ct);

            if (job is null)
                return NotFound("Job not found or not owned by this agent.");

            // TODO: Sende enum isimleri farklıysa uyarlayacaksın.
            job.Status = req.Success ? IntegrationJobStatus.Succeeded : IntegrationJobStatus.Failed;
            job.CompletedAt = DateTimeOffset.UtcNow;
            job.ErrorMessage = req.Message;
            job.ResultJson = req.ResultJson;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        private async Task<Guid?> ClaimNextJobIdAsync(Guid tenantId, Guid companyId, Guid agentMachineId, CancellationToken ct)
        {
            // Neden raw SQL?
            // EF ile "bul + güncelle" iki adım olursa, iki agent aynı job’u alabilir.
            // UPDLOCK/READPAST ile aynı satırın iki kez claim edilmesini engelleriz.

            const string sql = @"
DECLARE @id uniqueidentifier;

SELECT TOP (1) @id = j.Id
FROM IntegrationJobs j WITH (READPAST, UPDLOCK, ROWLOCK)
WHERE j.TenantId = @tenantId
  AND j.CompanyId = @companyId
  AND j.Status = @queuedStatus
  AND j.IsDeleted = 0
ORDER BY j.CreatedAt;

IF @id IS NOT NULL
BEGIN
    UPDATE IntegrationJobs
    SET Status = @inProgressStatus,
        AgentMachineId = @agentMachineId,
        StartedAt = SYSUTCDATETIME()
    WHERE Id = @id;
END

SELECT @id;
";

            // TODO: Sende status int değerleri farklıysa düzelt.
            // Önerilen: Queued=0, InProgress=1
            var pTenant = new SqlParameter("@tenantId", SqlDbType.UniqueIdentifier) { Value = tenantId };
            var pCompany = new SqlParameter("@companyId", SqlDbType.UniqueIdentifier) { Value = companyId };
            var pAgent = new SqlParameter("@agentMachineId", SqlDbType.UniqueIdentifier) { Value = agentMachineId };
            var pQueued = new SqlParameter("@queuedStatus", SqlDbType.Int) { Value = (int)IntegrationJobStatus.Queued };
            var pInProg = new SqlParameter("@inProgressStatus", SqlDbType.Int) { Value = (int)IntegrationJobStatus.InProgress };

            // EF Core 9: SqlQueryRaw ile scalar döndürmek mümkün
            var id = await _db.Database.SqlQueryRaw<Guid?>(sql, pTenant, pCompany, pQueued, pInProg, pAgent)
                .SingleAsync(ct);

            return id;
        }
    }

    public sealed class AgentJobDto
    {
        // Neden: Agent’ın job lifecycle yönetiminde ana kimlik.
        public Guid JobId { get; set; }

        // Neden: Agent hangi adaptörü/komutu çalıştıracağını buradan bilir (örn: "PostVoucher").
        public string CommandType { get; set; } = default!;

        // Neden: İşin tüm parametreleri tek payload içinde taşınır (program seçimi, fiş verisi, bağlantı id vb.)
        public string PayloadJson { get; set; } = default!;
    }

    public sealed class CompleteJobRequest
    {
        // Neden: Başarılı/başarısız durumunu DB’ye yansıtmak.
        public bool Success { get; set; }

        // Neden: İnsan okunabilir hata/başarı mesajı.
        public string? Message { get; set; }

        // Neden: Teknik çıktılar (örn: ERP fiş numarası, kayıt id’leri, hata stack) JSON olarak saklanabilir.
        public string? ResultJson { get; set; }
    }
}
