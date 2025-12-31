using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;


namespace Crm.Entities.Integration
{
    /// <summary>
    /// Bağlantı bilgilerini plaintext tutmamak için şifrelenmiş JSON.
    /// (Şifreleme Data/Services katmanında yapılır)
    /// </summary>
    public class ConnectionSecret : TenantEntity
    {
        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string EncryptedJson { get; set; } = default!;

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Notes { get; set; }
    }
}
