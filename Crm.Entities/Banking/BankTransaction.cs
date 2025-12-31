using Crm.Entities.Common;
using Crm.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Normalize edilmiş banka hareket satırı.
    /// </summary>
    public class BankTransaction : TenantEntity
    {
        public Guid ImportId { get; set; }
        public BankStatementImport Import { get; set; } = default!;

        public DateTime TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? ReferenceNo { get; set; }

        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string Description { get; set; } = default!;

        /// <summary>
        /// İşlem tutarı (+/-). Negatif: çıkış (varsayılan).
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// İşlem sonrası bakiye.
        /// </summary>
        public decimal BalanceAfter { get; set; }

        /// <summary>
        /// Kaynak dosyadaki satır no (idempotency/izlenebilirlik).
        /// </summary>
        public int RowNo { get; set; }

        // Eşleştirme
        public MappingStatus MappingStatus { get; set; } = MappingStatus.Unmapped;

        [MaxLength(EntityConstants.AccountCodeMax)]
        public string? SuggestedCounterAccountCode { get; set; }

        [MaxLength(EntityConstants.AccountCodeMax)]
        public string? ApprovedCounterAccountCode { get; set; }

        public Guid? AppliedRuleId { get; set; }

        /// <summary>
        /// 0..1 arası güven skoru (opsiyonel).
        /// </summary>
        public decimal? Confidence { get; set; }
    }
}
