using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Integration
{
    /// <summary>
    /// ERP/DB entegrasyon profili.
    /// Tenant bazlı veya Company bazlı olabilir.
    /// </summary>
    public class IntegrationProfile : TenantEntity
    {
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        /// <summary>
        /// Profil adı.
        /// Neden: Aynı firmada birden fazla bağlantı/profil olabilir (farklı şube, farklı DB, test/prod gibi).
        /// </summary>
        [Required, MaxLength(EntityConstants.NameMax)]
        public string Name { get; set; } = "Default";

        /// <summary>
        /// Adapter/driver seçimi.
        /// Neden: Aynı ProgramType içinde farklı çalışma şekilleri olabilir.
        /// Örn: "mssql" (doğrudan T-SQL), "netopenx" (Netsis), "logo-od" (Logo Object Designer), "playwright" (Luca).
        /// </summary>
        [Required, MaxLength(EntityConstants.CodeMax)]
        public string AdapterKey { get; set; } = "mssql";

        /// <summary>
        /// Non-secret ayarlar JSON.
        /// Neden: Server/Database gibi gizli olmayan ayarları burada tutarız.
        /// Şifre/parola gibi secret alanlar ConnectionSecret.EncryptedJson içinde tutulur.
        /// </summary>
        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string SettingsJson { get; set; } = "{}";

        public ProgramType ProgramType { get; set; }

        /// <summary>
        /// SecretId -> ConnectionSecret ile birebir ilişki.
        /// Neden: Şifreleri plaintext saklamamak için.
        /// </summary>
        public Guid SecretId { get; set; }
        public ConnectionSecret Secret { get; set; } = default!;

        [MaxLength(EntityConstants.CodeMax)]
        public string? BranchCode { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? WorkplaceCode { get; set; }

        /// <summary>
        /// Varsayılan profil.
        /// Neden: UI'da her seferinde seçim yapmadan otomatik profil seçimi için.
        /// </summary>
        public bool IsDefault { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
