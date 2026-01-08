using Crm.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Dispatch
{
    public class BulkFinancialDispatch : TenantEntity
    {
        [Required, MaxLength(EntityConstants.TitleMax)]
        public string Title { get; set; } = default!;
        public int FiscalYear { get; set; }
        public int? PeriodMonth { get; set; }
        [Required, MaxLength(EntityConstants.CodeMax)]
        public string DispatchType { get; set; } = default!;
        public DateTime ScheduledDate { get; set; }
        public DateTime? ExecutedDate { get; set; }
        [MaxLength(EntityConstants.MediumTextMax)]
        public string? TargetCompaniesFilterJson { get; set; }
        public int TotalCompanies { get; set; }
        public int SuccessfulDispatches { get; set; }
        public int FailedDispatches { get; set; }
        [MaxLength(EntityConstants.MediumTextMax)]
        public string? SummaryJson { get; set; }
        public DispatchStatus Status { get; set; } = DispatchStatus.Scheduled;
        public ICollection<DispatchItem> Items { get; set; } = new List<DispatchItem>();
    }
}
