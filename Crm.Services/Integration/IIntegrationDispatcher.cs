using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Services.Integration
{
    public interface IIntegrationDispatcher
    {
        Task<Guid> EnqueuePostVoucherAsync(Guid tenantId, Guid companyId, Guid integrationProfileId, Guid voucherDraftId, CancellationToken ct);
    }
}
