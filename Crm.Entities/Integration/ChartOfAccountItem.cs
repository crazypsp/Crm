using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Integration
{
    /// <summary>
    /// Hesap planı satırı (cache).
    ///
    /// Neden bu tablo var?
    /// - Muhasebe/ERP sistemlerinin hesap planı şeması/sorgusu farklıdır.
    /// - Banka hareketlerinde “açıklama -> muhasebe kodu” eşleştirmesi için hesap planına hızlı erişim gerekir.
    /// - Dış DB’ye her işlemde gitmek hem yavaştır hem de erişim sorunlarına açıktır.
    ///
    /// Bu tabloda "Company + ProgramType" bazında hesap planını CRM DB’de saklarız.
    /// </summary>
    public class ChartOfAccountItem : TenantEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public ProgramType ProgramType { get; set; }

        /// <summary>
        /// Muhasebe hesap kodu (örn: 102.01.001)
        /// </summary>
        [Required, MaxLength(EntityConstants.AccountCodeMax)]
        public string Code { get; set; } = default!;

        /// <summary>
        /// Muhasebe hesap adı (örn: BANKALAR - ... )
        /// </summary>
        [Required, MaxLength(EntityConstants.NameMax)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Bazı programlarda "aktif/pasif" veya "kullanım dışı" bilgisi olur.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Dış sistemdeki ID (varsa).
        /// Neden: Netsis/Logo gibi sistemlerde numeric ID ile referans vermek gerekebilir.
        /// </summary>
        [MaxLength(EntityConstants.CodeMax)]
        public string? ExternalId { get; set; }

        /// <summary>
        /// Hiyerarşik yapı gerekiyorsa parent kod.
        /// (MVP’de opsiyonel)
        /// </summary>
        [MaxLength(EntityConstants.AccountCodeMax)]
        public string? ParentCode { get; set; }
    }
}
