using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Api.Shared.Security
{
    public interface ICurrentUserContext
    {
        Guid UserId { get; }
        Guid TenantId { get; }
        Guid? CompanyId { get; }
        Guid? DealerId { get; }
        IReadOnlyCollection<string> Roles { get; }
    }
}
