using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Fiş satırı.
    /// </summary>
    public class VoucherDraftLine : TenantEntity
    {
        public Guid VoucherDraftId { get; set; }
        public VoucherDraft VoucherDraft { get; set; } = default!;

        public int LineNo { get; set; }

        [Required, MaxLength(EntityConstants.AccountCodeMax)]
        public string AccountCode { get; set; } = default!;

        [MaxLength(EntityConstants.NameMax)]
        public string? AccountName { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? LineDescription { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? CostCenterCode { get; set; }
    }
}
