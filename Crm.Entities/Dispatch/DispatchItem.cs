using Crm.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Dispatch
{
    public class DispatchItem : TenantEntity
    {
        public Guid BulkDispatchId { get; set; }
        public BulkFinancialDispatch BulkDispatch { get; set; } = default!;

        public Guid CompanyId { get; set; }
        // Company navigation property'si eklenmeli, ancak Company sınıfı farklı bir namespace'te.
        // Bu yüzden using ekleyip navigation property koyalım.
        public Crm.Entities.Tenancy.Company Company { get; set; } = default!;

        public DateTime? SentAt { get; set; }
        [MaxLength(EntityConstants.CodeMax)]
        public string? SentMethod { get; set; } // "EMAIL", "SMS", "CRM_NOTIFICATION"

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? ErrorMessage { get; set; }

        public bool IsSuccessful { get; set; }
    }
}