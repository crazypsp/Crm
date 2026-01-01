using Crm.Api.Messaging.Contracts;
using Crm.Api.Messaging.Infrastructure;
using Crm.Data;
using Crm.Entities.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Messaging.Controllers
{
    [ApiController]
    [Route("api/messaging/messages")]
    public sealed class MessagesController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public MessagesController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<PagedResult<MessageDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid threadId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            // Neden: Mesajlar büyür; sayfalama şart.
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;

            var q = _db.Messages.AsNoTracking()
                .Where(m => m.TenantId == tenantId && !m.IsDeleted);

            // ThreadId alan adı farklı olabilir: ThreadId / MessageThreadId
            // Önce ThreadId deneriz; patlarsa MessageThreadId
            try
            {
                q = q.Where(m => EF.Property<Guid>(m, "ThreadId") == threadId);
            }
            catch (InvalidOperationException)
            {
                q = q.Where(m => EF.Property<Guid>(m, "MessageThreadId") == threadId);
            }

            var total = await q.CountAsync(ct);

            // UI genelde “en yeni altta” ister; sayfalama için desc çekip sonra ters çeviriyoruz.
            var list = await q
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            list.Reverse();

            var dto = list.Select(m =>
            {
                var senderObj = EntityMap.TryGet(m, "SenderUserId", "UserId");
                var senderId = Guid.TryParse(senderObj?.ToString(), out var g) ? g : Guid.Empty;

                return new MessageDto
                {
                    Id = m.Id,
                    ThreadId = threadId,
                    SenderUserId = senderId,
                    Body = EntityMap.TryGetString(m, "Body", "Text", "Content"),
                    IsInternal = EntityMap.TryGetBool(m, "IsInternal"),
                    CreatedAt = EntityMap.TryGetCreatedAt(m)
                };
            }).ToList();

            return Ok(new PagedResult<MessageDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = dto
            });
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest req, CancellationToken ct)
        {
            // Neden: Thread’e yeni mesaj eklemek, yazışma akışının temelidir.
            // Önce thread var mı kontrol (tenant izolasyonu + soft delete)
            var threadExists = await _db.MessageThreads.AsNoTracking()
                .AnyAsync(t => t.Id == req.ThreadId && t.TenantId == req.TenantId && !t.IsDeleted, ct);

            if (!threadExists) return NotFound("Thread bulunamadı.");

            var msg = new Message
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(msg,
                ("ThreadId", req.ThreadId),
                ("MessageThreadId", req.ThreadId),
                ("SenderUserId", req.SenderUserId),
                ("UserId", req.SenderUserId),
                ("Body", req.Body),
                ("Text", req.Body),
                ("Content", req.Body),
                ("IsInternal", req.IsInternal)
            );

            _db.Messages.Add(msg);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = msg.Id });
        }
    }
}
