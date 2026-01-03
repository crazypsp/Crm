using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Api.Shared.Security
{
    /// <summary>
    /// Neden: Rol isimleri string olarak her yerde dağılırsa typo yüzünden yetki açığı oluşur.
    /// Bu sabitler policy tanımlarının tek kaynağıdır.
    /// </summary>
    public static class CrmRoles
    {
        public const string Admin = "Admin";
        public const string Dealer = "Dealer";
        public const string Accountant = "Accountant";           // Mali Müşavir
        public const string AccountantStaff = "AccountantStaff"; // Personel
        public const string CompanyUser = "Company";             // Firma kullanıcısı
    }
}
