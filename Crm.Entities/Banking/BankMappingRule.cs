using System.ComponentModel.DataAnnotations;
using Crm.Entities.Common;
using Crm.Entities.Enums;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Açıklama/pattern'e göre muhasebe karşı hesap kodu önerir.
    /// </summary>
    public class BankMappingRule : TenantEntity
    {
        [Required, MaxLength(EntityConstants.NameMax)]
        public string Name { get; set; } = default!;

        public Crm.Entities.Enums.MatchType MatchType { get; set; } = Crm.Entities.Enums.MatchType.Contains;

        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string Pattern { get; set; } = default!;

        public ProgramType? ProgramType { get; set; }
        public Guid? CompanyId { get; set; }

        [Required, MaxLength(EntityConstants.AccountCodeMax)]
        public string CounterAccountCode { get; set; } = default!;

        [MaxLength(EntityConstants.NameMax)]
        public string? CounterAccountName { get; set; }

        /// <summary>
        /// true: sadece çıkış, false: sadece giriş, null: fark etmez
        /// </summary>
        public bool? OnlyOutflow { get; set; }

        public int Priority { get; set; } = 100;
        public bool IsActive { get; set; } = true;
    }
}
