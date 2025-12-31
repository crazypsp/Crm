using Crm.Entities.Common;
using Crm.Entities.Documents;
using Crm.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Messaging
{
    /// <summary>
    /// Thread içindeki mesaj.
    /// </summary>
    public class Message : TenantEntity
    {
        public Guid ThreadId { get; set; }
        public MessageThread Thread { get; set; } = default!;

        public Guid SenderUserId { get; set; }
        public ApplicationUser SenderUser { get; set; } = default!;

        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string Body { get; set; } = default!;

        public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;

        public Guid? AttachmentFileId { get; set; }
        public DocumentFile? AttachmentFile { get; set; }
    }
}
