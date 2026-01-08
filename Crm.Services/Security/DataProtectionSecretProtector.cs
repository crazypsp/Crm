using Microsoft.AspNetCore.DataProtection;

namespace Crm.Services.Security
{
    public sealed class DataProtectionSecretProtector : ISecretProtector
    {
        private readonly IDataProtector _protector;

        public DataProtectionSecretProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("CrmSuite.ConnectionSecrets.v1");
        }

        public string Protect(string plainText) => _protector.Protect(plainText);
        public string Unprotect(string protectedText) => _protector.Unprotect(protectedText);
    }
}