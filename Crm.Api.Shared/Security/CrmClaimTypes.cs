using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Api.Shared.Security
{
    /// <summary>
    /// Neden: Tenant/Company/Dealer gibi scope bilgileri request ile gönderilirse spoof edilebilir.
    /// Bu nedenle her API bu değerleri JWT içindeki claim'lerden okur.
    /// </summary>
    public static class CrmClaimTypes
    {
        public const string TenantId = "crm_tenant_id";
        public const string CompanyId = "crm_company_id";
        public const string DealerId = "crm_dealer_id";
    }
}
