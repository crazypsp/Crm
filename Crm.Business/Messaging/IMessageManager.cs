using Crm.Entities.Enums;
using Crm.Entities.Messaging;

namespace Crm.Business.Messaging
{
    public interface IMessageManager
    {
        Task<MessageThread> CreateThreadAsync(Guid tenantId, Guid? companyId, MessageThreadType type, Guid? relatedEntityId, string title, CancellationToken ct);
        Task<Message> PostMessageAsync(Guid tenantId, Guid threadId, Guid senderUserId, string body, Guid? attachmentFileId, CancellationToken ct);
    }
}
