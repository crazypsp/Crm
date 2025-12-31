using Crm.Entities.Common;
using Crm.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Messaging
{
    /// <summary>
    /// Konu bazlı yazışma (thread). Firma-personel-müşavir iletişimi.
    /// </summary>
    public class MessageThread : TenantEntity
    {
        public MessageThreadType ThreadType { get; set; } = MessageThreadType.General;

        public Guid? CompanyId { get; set; }

        /// <summary>
        /// İlişkili entity (DocumentRequestId, BankImportId, WorkTaskId vb.)
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        [Required, MaxLength(EntityConstants.TitleMax)]
        public string Title { get; set; } = default!;

        public ICollection<ThreadParticipant> Participants { get; set; } = new List<ThreadParticipant>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
