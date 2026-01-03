using System.Text.Json;
using Crm.Business.Common;
using Crm.Business.Common.Exceptions;
using Crm.Data;
using Crm.Entities.Enums;
using Crm.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Integration;

/// <summary>
/// Neden: Integration job üretme/okuma/işaretleme gibi iş kuralları controller içinde değil,
/// business katmanında toplanmalı.
/// - Controller: HTTP sözleşmesi ve auth
/// - Business: kurallar + validasyon + erişim kontrolü + transaction
/// - Data: EF Core kalıcılık
/// </summary>
public sealed class IntegrationJobManager
{
    private readonly CrmDbContext _db;

    public IntegrationJobManager(CrmDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Neden: Agent'a iş üretmenin tek, güvenli yolu.
    /// Bu method:
    /// - tenant/company scope doğrulaması yapar
    /// - commandType doğrular
    /// - payload json serialize eder
    /// - job'ı "Queued" olarak kuyruk tablosuna yazar
    /// </summary>
    public async Task<Guid> EnqueueAsync(
        Guid tenantId,
        Guid? companyId,
        string commandType,
        object payload,
        Guid? agentMachineId = null,
        CancellationToken ct = default)
    {
        // Guard: temel input doğrulaması (null/boş)
        Guard.Against(tenantId == Guid.Empty, "tenantId is required.");
        Guard.AgainstNullOrWhiteSpace(commandType, nameof(commandType));
        Guard.AgainstNull(payload, nameof(payload));

        // Neden: CompanyId verilmişse, gerçekten bu tenant altında var mı?
        if (companyId is not null)
        {
            var exists = await _db.Companies.AsNoTracking()
                .AnyAsync(x => x.TenantId == tenantId && x.Id == companyId.Value && !x.IsDeleted, ct);

            if (!exists)
                throw new Common.NotFoundException("Company not found for given tenant.");
        }

        // Neden: AgentMachineId verilmişse bu tenant'a ait mi?
        // AgentMachine entity senin modelinde varsa bu kontrol değerlidir; yoksa bu bloğu kaldırabilirsin.
        if (agentMachineId is not null)
        {
            var exists = await _db.AgentMachines.AsNoTracking()
                .AnyAsync(x => x.TenantId == tenantId && x.Id == agentMachineId.Value && !x.IsDeleted, ct);

            if (!exists)
                throw new Common.NotFoundException("Agent machine not found for given tenant.");
        }

        // Neden: Payload schema versiyonlaması için JSON serializer ayarını tek noktadan yönetiriz.
        var payloadJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });

        // Neden: Status enum değerleri farklı isimlerde olabilir ama default olarak (0) Queued bekliyoruz.
        // Eğer senin enum dizilimin farklıysa, aşağıdaki satırı kendi Queued enum'una göre değiştir.
        var job = new IntegrationJob
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,

            CompanyId = companyId,
            AgentMachineId = agentMachineId,

            // Neden: Business katmanının ve Agent'ın hangi işi çalıştıracağını belirler.
            CommandType = commandType.Trim(),

            PayloadJson = payloadJson,

            // Kuyruk defaultları
            Status = (IntegrationJobStatus)0, // Queued
            Attempts = 0,
            LockedBy = null,
            LockedUntil = null,
            LastError = null,
            ResultJson = null,

            CreatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };

        _db.IntegrationJobs.Add(job);
        await _db.SaveChangesAsync(ct);

        return job.Id;
    }

    /// <summary>
    /// Neden: UI veya servislerin job durumunu görmesi için tek entry-point.
    /// Business layer'da olduğundan scope kontrolleri tek yerde yapılır.
    /// </summary>
    public async Task<IntegrationJob> GetAsync(Guid tenantId, Guid jobId, CancellationToken ct = default)
    {
        Guard.Against(tenantId == Guid.Empty, "tenantId is required.");
        Guard.Against(jobId == Guid.Empty, "jobId is required.");

        var job = await _db.IntegrationJobs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == jobId && !x.IsDeleted, ct);

        if (job is null)
            throw new Common.NotFoundException("Integration job not found.");

        return job;
    }

    /// <summary>
    /// Neden: Admin/Support "dead-letter" işleri incelemek isteyecek.
    /// Bu listeleme API/Reporting tarafında kullanılabilir.
    /// </summary>
    public async Task<IReadOnlyList<IntegrationJob>> ListDeadLettersAsync(
        Guid tenantId,
        int take = 50,
        CancellationToken ct = default)
    {
        Guard.Against(tenantId == Guid.Empty, "tenantId is required.");
        Guard.Against(take <= 0 || take > 500, "take must be between 1 and 500.");

        var deadLetter = (IntegrationJobStatus)4; // DeadLetter

        return await _db.IntegrationJobs.AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted && x.Status == deadLetter)
            .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
            .Take(take)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Neden: DeadLetter olan bir işi tekrar kuyruğa almak için (support operasyonu).
    /// </summary>
    public async Task RequeueAsync(Guid tenantId, Guid jobId, CancellationToken ct = default)
    {
        Guard.Against(tenantId == Guid.Empty, "tenantId is required.");
        Guard.Against(jobId == Guid.Empty, "jobId is required.");

        var job = await _db.IntegrationJobs
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == jobId && !x.IsDeleted, ct);

        if (job is null)
            throw new Common.NotFoundException("Integration job not found.");

        // DeadLetter değilse requeue etmiyoruz (operasyonel hata riskini azaltmak için)
        var deadLetter = (IntegrationJobStatus)4;
        if (job.Status != deadLetter)
            throw new Common.ForbiddenException("Only dead-letter jobs can be re-queued.");

        // Neden: Yeni bir deneme "temiz state" ile başlamalı
        job.Status = (IntegrationJobStatus)0; // Queued
        job.Attempts = 0;
        job.LockedBy = null;
        job.LockedUntil = null;
        job.LastError = null;
        job.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(ct);
    }
}
