using Crm.Entities.Common;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Bir banka satırı ile üretilen fiş satırlarının izlenebilirliği.
    /// </summary>
    public class VoucherDraftItem : TenantEntity
    {
        public Guid VoucherDraftId { get; set; }
        public VoucherDraft VoucherDraft { get; set; } = default!;

        public Guid BankTransactionId { get; set; }
        public BankTransaction BankTransaction { get; set; } = default!;

        public int FirstLineNo { get; set; }
        public int LastLineNo { get; set; }
    }
}
