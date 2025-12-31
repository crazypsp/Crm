using Crm.Entities.Common;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Firmanın bankası ve muhasebe banka hesap kodu.
    /// </summary>
    public class BankAccount : TenantEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        [Required, MaxLength(EntityConstants.NameMax)]
        public string BankName { get; set; } = default!;

        [MaxLength(EntityConstants.IbanMax)]
        public string? Iban { get; set; }

        [Required, MaxLength(EntityConstants.CurrencyCodeMax)]
        public string Currency { get; set; } = "TRY";

        /// <summary>
        /// Muhasebe banka hesap kodu (örn: 102.01.001)
        /// </summary>
        [Required, MaxLength(EntityConstants.AccountCodeMax)]
        public string AccountingBankAccountCode { get; set; } = default!;

        [MaxLength(EntityConstants.NameMax)]
        public string? AccountingBankAccountName { get; set; }
    }
}
