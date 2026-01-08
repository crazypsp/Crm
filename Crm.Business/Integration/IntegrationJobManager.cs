using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Enums;
using Crm.Entities.Integration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Crm.Business.Integration
{
    public sealed class IntegrationJobManager : IIntegrationJobManager
    {
        private readonly CrmDbContext _db;

        public IntegrationJobManager(CrmDbContext db)
        {
            _db = db;
        }

        public async Task<IntegrationJob> EnqueueJobAsync(
            Guid tenantId,
            Guid? companyId,
            Guid? agentMachineId,
            string commandType,
            string payloadJson,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotBlank(commandType, nameof(commandType));
            Guard.NotBlank(payloadJson, nameof(payloadJson));

            if (companyId.HasValue)
            {
                var companyExists = await _db.Companies.AsNoTracking()
                    .AnyAsync(x => x.TenantId == tenantId && x.Id == companyId.Value && !x.IsDeleted, ct);

                if (!companyExists)
                    throw new NotFoundException("Company not found for given tenant.");
            }

            if (agentMachineId.HasValue)
            {
                var agentExists = await _db.AgentMachines.AsNoTracking()
                    .AnyAsync(x => x.TenantId == tenantId && x.Id == agentMachineId.Value && !x.IsDeleted, ct);

                if (!agentExists)
                    throw new NotFoundException("Agent machine not found for given tenant.");
            }

            var job = new IntegrationJob
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CompanyId = companyId,
                AgentMachineId = agentMachineId,
                CommandType = commandType.Trim(),
                PayloadJson = payloadJson,
                Status = IntegrationJobStatus.Queued,
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

            return job;
        }

        public async Task MarkSucceededAsync(
            Guid tenantId,
            Guid jobId,
            string resultJson,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(jobId, nameof(jobId));

            var job = await _db.IntegrationJobs
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == jobId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Integration job not found.");

            job.Status = IntegrationJobStatus.Succeeded;
            job.ResultJson = resultJson;
            job.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        public async Task MarkFailedAsync(
            Guid tenantId,
            Guid jobId,
            string errorMessage,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(jobId, nameof(jobId));
            Guard.NotBlank(errorMessage, nameof(errorMessage));

            var job = await _db.IntegrationJobs
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == jobId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Integration job not found.");

            job.Status = IntegrationJobStatus.Failed;
            job.LastError = errorMessage;
            job.Attempts++;
            job.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        public async Task<IntegrationJob> GetAsync(
            Guid tenantId,
            Guid jobId,
            CancellationToken ct = default)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(jobId, nameof(jobId));

            var job = await _db.IntegrationJobs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == jobId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Integration job not found.");

            return job;
        }

        public async Task<IReadOnlyList<IntegrationJob>> ListDeadLettersAsync(
            Guid tenantId,
            int take = 50,
            CancellationToken ct = default)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.InRange(take, 1, 500, nameof(take));

            return await _db.IntegrationJobs.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted && x.Status == IntegrationJobStatus.DeadLetter)
                .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
                .Take(take)
                .ToListAsync(ct);
        }

        public async Task RequeueAsync(
            Guid tenantId,
            Guid jobId,
            CancellationToken ct = default)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(jobId, nameof(jobId));

            var job = await _db.IntegrationJobs
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == jobId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Integration job not found.");

            if (job.Status != IntegrationJobStatus.DeadLetter)
                throw new ForbiddenException("Only dead-letter jobs can be re-queued.");

            job.Status = IntegrationJobStatus.Queued;
            job.Attempts = 0;
            job.LockedBy = null;
            job.LockedUntil = null;
            job.LastError = null;
            job.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }
    }
}