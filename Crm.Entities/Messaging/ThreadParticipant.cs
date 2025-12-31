using Crm.Entities.Common;
using Crm.Entities.Identity;

namespace Crm.Entities.Messaging
{
    /// <summary>
    /// Thread'e dahil olan kullanıcı.
    /// </summary>
    public class ThreadParticipant : TenantEntity
    {
        public Guid ThreadId { get; set; }
        public MessageThread Thread { get; set; } = default!;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
    }
}
