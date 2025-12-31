using Crm.Entities.Common;
using Crm.Entities.Identity;

namespace Crm.Entities.Work
{
    /// <summary>
    /// Görevin hangi kullanıcı(lar)a atandığı.
    /// </summary>
    public class WorkTaskAssignment : TenantEntity
    {
        public Guid WorkTaskId { get; set; }
        public WorkTask WorkTask { get; set; } = default!;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public bool IsOwner { get; set; }
    }
}
