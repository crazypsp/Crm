using Crm.Entities.Work;

namespace Crm.Business.Work
{
    public interface IWorkTaskManager
    {
        Task<WorkTask> CreateTaskAsync(
            Guid tenantId,
            Guid? companyId,
            string title,
            string? description,
            DateTimeOffset? dueAt,
            int priority,
            CancellationToken ct);

        Task AssignUserAsync(
            Guid tenantId,
            Guid taskId,
            Guid userId,
            bool isOwner,
            CancellationToken ct);

        Task SetStatusAsync(
            Guid tenantId,
            Guid taskId,
            int status,
            CancellationToken ct);
    }
}
