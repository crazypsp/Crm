using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Api.Shared.Security
{
    public static class CrmPolicies
    {
        public const string AdminOnly = "AdminOnly";
        public const string DealerOnly = "DealerOnly";
        public const string AccountantOnly = "AccountantOnly";
        public const string StaffOnly = "StaffOnly";
        public const string CompanyOnly = "CompanyOnly";
    }
}
