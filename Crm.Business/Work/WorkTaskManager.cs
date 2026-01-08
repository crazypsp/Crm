using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Enums;
using Crm.Entities.Work;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Work
{
    public sealed class WorkTaskManager : IWorkTaskManager
    {
        private readonly CrmDbContext _db;

        public WorkTaskManager(CrmDbContext db)
        {
            _db = db;
        }

        public async Task<WorkTask> CreateTaskAsync(
            Guid tenantId,
            Guid? companyId,
            string title,
            string? description,
            DateTimeOffset? dueAt,
            int priority,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotBlank(title, nameof(title));

            if (companyId.HasValue)
            {
                var companyOk = await _db.Companies
                    .AnyAsync(x => x.Id == companyId && x.TenantId == tenantId && !x.IsDeleted, ct);
                if (!companyOk)
                    throw new ForbiddenException("Firma bulunamadı veya tenant yetkisi yok.");
            }

            var task = new WorkTask
            {
                TenantId = tenantId,
                CompanyId = companyId,
                Title = title.Trim(),
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                DueAt = dueAt,
                Priority = priority <= 0 ? 3 : priority,
                Status = WorkTaskStatus.Open
            };

            _db.WorkTasks.Add(task);
            await _db.SaveChangesAsync(ct);

            return task;
        }

        public async Task AssignUserAsync(
            Guid tenantId,
            Guid taskId,
            Guid userId,
            bool isOwner,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(taskId, nameof(taskId));
            Guard.NotEmpty(userId, nameof(userId));

            var task = await _db.WorkTasks
                .FirstOrDefaultAsync(x => x.Id == taskId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Görev bulunamadı.");

            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == userId, ct)
                ?? throw new NotFoundException("Kullanıcı bulunamadı.");

            if (user.TenantId != tenantId)
                throw new ForbiddenException("Kullanıcı bu tenant'a ait değil.");

            var exists = await _db.WorkTaskAssignments
                .AnyAsync(x => x.WorkTaskId == taskId && x.UserId == userId && x.TenantId == tenantId, ct);

            if (!exists)
            {
                _db.WorkTaskAssignments.Add(new WorkTaskAssignment
                {
                    TenantId = tenantId,
                    WorkTaskId = taskId,
                    UserId = userId,
                    IsOwner = isOwner
                });

                await _db.SaveChangesAsync(ct);
            }
        }

        public async Task SetStatusAsync(
            Guid tenantId,
            Guid taskId,
            int status,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(taskId, nameof(taskId));

            var task = await _db.WorkTasks
                .FirstOrDefaultAsync(x => x.Id == taskId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Görev bulunamadı.");

            if (!Enum.IsDefined(typeof(WorkTaskStatus), status))
                throw new ValidationException("Geçersiz görev durumu.");

            task.Status = (WorkTaskStatus)status;
            await _db.SaveChangesAsync(ct);
        }
    }
}
