using Crm.Entities.Integration;

namespace Crm.Business.Integration
{
    public interface IIntegrationJobManager
    {
        Task<IntegrationJob> EnqueueJobAsync(
            Guid tenantId,
            Guid? companyId,
            Guid? agentMachineId,
            string commandType,
            string payloadJson,
            CancellationToken ct);

        Task MarkSucceededAsync(
            Guid tenantId,
            Guid jobId,
            string resultJson,
            CancellationToken ct);

        Task MarkFailedAsync(
            Guid tenantId,
            Guid jobId,
            string errorMessage,
            CancellationToken ct);

        Task<IntegrationJob> GetAsync(
            Guid tenantId,
            Guid jobId,
            CancellationToken ct = default);

        Task<IReadOnlyList<IntegrationJob>> ListDeadLettersAsync(
            Guid tenantId,
            int take = 50,
            CancellationToken ct = default);

        Task RequeueAsync(
            Guid tenantId,
            Guid jobId,
            CancellationToken ct = default);
    }
}