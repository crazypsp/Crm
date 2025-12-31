using Crm.Entities.Common;
using Crm.Entities.Tenancy;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Identity
{
    /// <summary>
    /// Sisteme giriş yapan kullanıcı.
    /// IdentityUser<Guid> üzerinden gelir.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(EntityConstants.NameMax)]
        public string? FullName { get; set; }

        /// <summary>
        /// Kullanıcının varsayılan scope’u (opsiyonel): login sonrası hangi bağlamla başlasın.
        /// Admin için null olabilir.
        /// </summary>
        public Guid? DealerId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? CompanyId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigations (opsiyonel)
        public Dealer? Dealer { get; set; }
        public Tenant? Tenant { get; set; }
        public Company? Company { get; set; }

        public ICollection<UserMembership> Memberships { get; set; } = new List<UserMembership>();
    }
}
