using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Enums;
using Crm.Entities.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Messaging
{
    public sealed class MessageManager : IMessageManager
    {
        private readonly CrmDbContext _db;
        public MessageManager(CrmDbContext db) => _db = db;

        public async Task<MessageThread> CreateThreadAsync(Guid tenantId, Guid? companyId, MessageThreadType type, Guid? relatedEntityId, string title, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotBlank(title, nameof(title));

            if (companyId.HasValue)
            {
                var ok = await _db.Companies.AnyAsync(x => x.Id == companyId && x.TenantId == tenantId && !x.IsDeleted, ct);
                if (!ok) throw new ForbiddenException("Firma bulunamadı veya tenant yetkisi yok.");
            }

            var thread = new MessageThread
            {
                TenantId = tenantId,
                CompanyId = companyId,
                ThreadType = type,
                RelatedEntityId = relatedEntityId,
                Title = title.Trim()
            };

            _db.MessageThreads.Add(thread);
            await _db.SaveChangesAsync(ct);
            return thread;
        }

        public async Task<Message> PostMessageAsync(Guid tenantId, Guid threadId, Guid senderUserId, string body, Guid? attachmentFileId, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(threadId, nameof(threadId));
            Guard.NotEmpty(senderUserId, nameof(senderUserId));
            Guard.NotBlank(body, nameof(body));

            var thread = await _db.MessageThreads.FirstOrDefaultAsync(x => x.Id == threadId && x.TenantId == tenantId && !x.IsDeleted, ct)
                         ?? throw new NotFoundException("Thread bulunamadı.");

            var sender = await _db.Users.FirstOrDefaultAsync(x => x.Id == senderUserId, ct)
                         ?? throw new NotFoundException("Gönderen kullanıcı bulunamadı.");

            // MVP: sender.TenantId kontrolü. İleri: Membership/policy.
            if (sender.TenantId != tenantId)
                throw new ForbiddenException("Kullanıcı tenant yetkisine sahip değil.");

            var msg = new Message
            {
                TenantId = tenantId,
                ThreadId = thread.Id,
                SenderUserId = senderUserId,
                Body = body.Trim(),
                AttachmentFileId = attachmentFileId
            };

            _db.Messages.Add(msg);
            await _db.SaveChangesAsync(ct);
            return msg;
        }
    }
}
