using System.Text;
using System.Text.Json;
using Crm.Services.Security;

namespace Crm.Api.Integration.Security
{
    public static class SecretCrypto
    {
        /// <summary>
        /// Settings -> JSON -> Protect -> Encrypted string
        /// Neden: ConnectionSecret.EncryptedJson alanı string ve ISecretProtector string tabanlı çalışıyor.
        /// </summary>
        public static string ProtectSettings(ISecretProtector protector, Dictionary<string, string> settings)
        {
            var json = JsonSerializer.Serialize(settings);
            return protector.Protect(json);
        }

        /// <summary>
        /// Encrypted string -> Unprotect -> JSON -> Settings
        /// Neden: Test connection / adapter çalıştırma gibi işlemlerde ayarları okumamız gerekebilir.
        /// </summary>
        public static Dictionary<string, string> UnprotectSettings(ISecretProtector protector, string encryptedJson)
        {
            if (string.IsNullOrWhiteSpace(encryptedJson))
                return new Dictionary<string, string>();

            var json = protector.Unprotect(encryptedJson);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
    }
}
