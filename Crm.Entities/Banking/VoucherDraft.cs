using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Banka hareketlerinden oluşturulan fiş taslağı (ERP'ye yazmadan önce).
    /// </summary>
    public class VoucherDraft : TenantEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public Guid ImportId { get; set; }
        public BankStatementImport Import { get; set; } = default!;

        public DateTime VoucherDate { get; set; }

        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string Description { get; set; } = default!;

        /// <summary>
        /// Banka hesabı muhasebe kodu (örn 102.01.001)
        /// </summary>
        [Required, MaxLength(EntityConstants.AccountCodeMax)]
        public string BankAccountCode { get; set; } = default!;

        public VoucherDraftStatus Status { get; set; } = VoucherDraftStatus.Draft;

        public ICollection<VoucherDraftLine> Lines { get; set; } = new List<VoucherDraftLine>();
        public ICollection<VoucherDraftItem> Items { get; set; } = new List<VoucherDraftItem>();
    }
}
