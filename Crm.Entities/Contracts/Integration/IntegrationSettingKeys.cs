using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Contracts.Integration
{
    /// <summary>
    /// IntegrationProfile.SettingsJson içinde kullanılacak anahtarlar.
    ///
    /// Neden ayrı bir sınıf?
    /// - Magic string'leri tek yerde tutarak typo kaynaklı bug'ları azaltır.
    /// - Hem API'ler hem Agent hem de Web UI aynı anahtar isimlerini kullanır.
    /// </summary>
    public static class IntegrationSettingKeys
    {
        // MSSQL bağlantısı (Zirve, ZirveGenel, ZirveMaliMusavir, Micro, LogoGo3 vb.)
        public const string SqlServer = "Sql.Server";             // DESKTOP-...\\INSTANCE
        public const string SqlDatabase = "Sql.Database";         // Zirve MMM: her mükellef db adı
        public const string SqlAuth = "Sql.Auth";                 // "Windows" | "Sql"
        public const string SqlUser = "Sql.User";                 // Sql auth ise
        public const string SqlPasswordSecretKey = "Sql.PasswordSecretKey"; // Secret içinde hangi key ile saklandı?

        // Hesap planı çekme
        public const string ChartOfAccountsQuery = "ChartOfAccounts.Query"; // opsiyonel override (bilmiyorsak boş kalabilir)

        // Zirve MMM özel
        public const string ZirveMmmDbMode = "ZirveMMM.DbMode";   // "PerCompanyDb" gibi (opsiyonel)
    }
}

