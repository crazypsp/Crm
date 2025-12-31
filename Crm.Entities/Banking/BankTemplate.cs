using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Excel/PDF kolon eşleme şablonu.
    /// ColumnMapJson: date, desc, amount, balance, ref, valueDate gibi alanları başlıklara map eder.
    /// </summary>
    public class BankTemplate : TenantEntity
    {
        [Required, MaxLength(EntityConstants.NameMax)]
        public string Name { get; set; } = default!;

        [MaxLength(EntityConstants.NameMax)]
        public string? BankName { get; set; }

        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string ColumnMapJson { get; set; } = default!;

        [Required, MaxLength(EntityConstants.CodeMax)]
        public string CultureName { get; set; } = "tr-TR";

        public bool AmountNegativeMeansOutflow { get; set; } = true;

        public bool IsActive { get; set; } = true;
    }
}
