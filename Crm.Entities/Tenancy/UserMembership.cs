using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Identity;

namespace Crm.Entities.Tenancy
{

    /// <summary>
    /// Kullanıcının hangi scope içinde hangi rolde olduğunu tutar.
    /// Bir kullanıcı birden fazla tenant/firma scope'unda rol alabilir.
    /// </summary>
    public class UserMembership : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public MembershipRole Role { get; set; }

        // Scope bilgisi:
        public Guid? DealerId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? CompanyId { get; set; }

        public bool IsPrimary { get; set; } = true;
    }
}
