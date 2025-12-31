using Crm.Entities.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Business.Integration
{
    public interface IIntegrationJobManager
    {
        Task<IntegrationJob> EnqueueJobAsync(Guid tenantId, Guid? companyId, Guid? agentMachineId, string commandType, string payloadJson, CancellationToken ct);
        Task MarkSucceededAsync(Guid tenantId, Guid jobId, string resultJson, CancellationToken ct);
        Task MarkFailedAsync(Guid tenantId, Guid jobId, string errorMessage, CancellationToken ct);
    }
}
