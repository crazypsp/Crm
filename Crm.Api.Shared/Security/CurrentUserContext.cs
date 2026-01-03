using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Api.Shared.Security
{
    /// <summary>
    /// Neden: Controller'larda sürekli HttpContext.User parsing yapmak hem hata üretir hem tekrar eder.
    /// Bu servis tek noktadan kullanıcı kimliğini ve scope bilgisini sağlar.
    /// </summary>
    public sealed class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUserContext(IHttpContextAccessor http) => _http = http;

        public Guid UserId => ReadGuid(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("UserId claim missing.");

        public Guid TenantId => ReadGuid(CrmClaimTypes.TenantId)
            ?? throw new UnauthorizedAccessException("TenantId claim missing.");

        public Guid? CompanyId => ReadGuid(CrmClaimTypes.CompanyId);
        public Guid? DealerId => ReadGuid(CrmClaimTypes.DealerId);

        public IReadOnlyCollection<string> Roles =>
            _http.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray()
            ?? Array.Empty<string>();

        private Guid? ReadGuid(string claimType)
        {
            var v = _http.HttpContext?.User?.FindFirst(claimType)?.Value;
            return Guid.TryParse(v, out var g) ? g : null;
        }
    }
}
