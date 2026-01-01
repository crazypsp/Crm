using Crm.Entities.Enums;

namespace Crm.Api.Integration.Contracts
{
    /// <summary>
    /// Profil oluşturma/güncelleme isteği.
    /// Neden: IntegrationProfile entity'si sadece program türü ve şube/işyeri gibi meta alanları tutuyor.
    /// Bağlantı detayları ise ConnectionSecret.EncryptedJson içinde şifreli JSON olarak tutuluyor.
    /// </summary>
    public sealed class UpsertIntegrationProfileRequest
    {
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; } // Entity'de nullable

        public ProgramType ProgramType { get; set; }

        public string? BranchCode { get; set; }
        public string? WorkplaceCode { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Bağlantı ayarları (genel amaçlı).
        /// Neden: Luca/Logo/Netsis/Zirve/Micro farklı parametreler ister.
        /// Bu alanlar JSON'a çevrilip Secret.EncryptedJson içinde şifrelenerek saklanır.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; } = new();
    }

    /// <summary>
    /// Profil listeleme/okuma DTO'su.
    /// Neden: Secret içeriğini geri döndürmek güvenlik açısından doğru değil.
    /// UI sadece "profil var mı / aktif mi / hangi program" gibi metayı görür.
    /// </summary>
    public sealed class IntegrationProfileDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }

        public ProgramType ProgramType { get; set; }

        public Guid SecretId { get; set; }

        public string? BranchCode { get; set; }
        public string? WorkplaceCode { get; set; }

        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    /// <summary>
    /// Sadece secret güncelleme isteği (profil metası değişmeden).
    /// Neden: UI'da "şifre / bağlantı" alanlarını ayrı yönetmek sık kullanılan bir ihtiyaçtır.
    /// </summary>
    public sealed class UpdateSecretRequest
    {
        public Guid TenantId { get; set; }
        public Dictionary<string, string> Settings { get; set; } = new();
        public string? Notes { get; set; }
    }

    public sealed class TestConnectionRequest
    {
        public Guid TenantId { get; set; }
        public Guid IntegrationProfileId { get; set; }
    }

    public sealed class TestConnectionResultDto
    {
        public bool Ok { get; set; }
        public string? Message { get; set; }
    }
}
