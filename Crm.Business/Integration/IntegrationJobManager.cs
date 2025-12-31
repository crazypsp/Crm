using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Enums;
using Crm.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Integration
{
    public sealed class IntegrationJobManager : IIntegrationJobManager
    {
        private readonly CrmDbContext _db;
        public IntegrationJobManager(CrmDbContext db) => _db = db;

        public async Task<IntegrationJob> EnqueueJobAsync(Guid tenantId, Guid? companyId, Guid? agentMachineId, string commandType, string payloadJson, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotBlank(commandType, nameof(commandType));
            Guard.NotBlank(payloadJson, nameof(payloadJson));

            if (companyId.HasValue)
            {
                var ok = await _db.Companies.AnyAsync(x => x.Id == companyId && x.TenantId == tenantId && !x.IsDeleted, ct);
                if (!ok) throw new ForbiddenException("Firma bulunamadı veya tenant yetkisi yok.");
            }

            if (agentMachineId.HasValue)
            {
                var ok = await _db.AgentMachines.AnyAsync(x => x.Id == agentMachineId && x.TenantId == tenantId && !x.IsDeleted, ct);
                if (!ok) throw new ForbiddenException("Agent machine bulunamadı veya tenant yetkisi yok.");
            }

            var job = new IntegrationJob
            {
                TenantId = tenantId,
                CompanyId = companyId,
                AgentMachineId = agentMachineId,
                CommandType = commandType.Trim(),
                PayloadJson = payloadJson,
                Status = IntegrationJobStatus.Queued
            };

            _db.IntegrationJobs.Add(job);
            await _db.SaveChangesAsync(ct);
            return job;
        }

        public async Task MarkSucceededAsync(Guid tenantId, Guid jobId, string resultJson, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(jobId, nameof(jobId));

            var job = await _db.IntegrationJobs.FirstOrDefaultAsync(x => x.Id == jobId && x.TenantId == tenantId && !x.IsDeleted, ct)
                      ?? throw new NotFoundException("Job bulunamadı.");

            job.Status = IntegrationJobStatus.Succeeded;
            job.ResultJson = resultJson;
            job.CompletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        public async Task MarkFailedAsync(Guid tenantId, Guid jobId, string errorMessage, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(jobId, nameof(jobId));

            var job = await _db.IntegrationJobs.FirstOrDefaultAsync(x => x.Id == jobId && x.TenantId == tenantId && !x.IsDeleted, ct)
                      ?? throw new NotFoundException("Job bulunamadı.");

            job.Status = IntegrationJobStatus.Failed;
            job.ErrorMessage = errorMessage;
            job.CompletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }
    }
}
