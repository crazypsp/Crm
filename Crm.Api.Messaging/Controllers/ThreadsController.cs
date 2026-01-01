using Crm.Api.Messaging.Contracts;
using Crm.Api.Messaging.Infrastructure;
using Crm.Data;
using Crm.Entities.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Messaging.Controllers
{
    [ApiController]
    [Route("api/messaging/threads")]
    public sealed class ThreadsController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public ThreadsController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<PagedResult<ThreadDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid companyId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            // Neden: Firma bazlı thread listesi UI’da ana ekran olur.
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;

            var q = _db.MessageThreads.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            // CompanyId alan adını bilmiyoruz; EF.Property ile okuyoruz.
            q = q.Where(x => EF.Property<Guid>(x, "CompanyId") == companyId);

            var total = await q.CountAsync(ct);

            var threads = await q
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            // Katılımcı sayısı: MVP’de ayrı query ile alınır.
            var threadIds = threads.Select(t => t.Id).ToList();

            var participantCounts = await _db.ThreadParticipants.AsNoTracking()
                .Where(p => p.TenantId == tenantId && !p.IsDeleted && threadIds.Contains(EF.Property<Guid>(p, "ThreadId")))
                .GroupBy(p => EF.Property<Guid>(p, "ThreadId"))
                .Select(g => new { ThreadId = g.Key, Cnt = g.Count() })
                .ToDictionaryAsync(x => x.ThreadId, x => x.Cnt, ct);

            // Son mesaj zamanı (varsa): MVP’de mesaj tablosundan max alınır.
            var lastMessageTimes = await _db.Messages.AsNoTracking()
                .Where(m => m.TenantId == tenantId && !m.IsDeleted && threadIds.Contains(EF.Property<Guid>(m, "ThreadId")))
                .GroupBy(m => EF.Property<Guid>(m, "ThreadId"))
                .Select(g => new { ThreadId = g.Key, LastAt = g.Max(x => x.CreatedAt) })
                .ToDictionaryAsync(x => x.ThreadId, x => (DateTimeOffset?)x.LastAt, ct);

            var dto = threads.Select(t => new ThreadDto
            {
                Id = t.Id,
                TenantId = t.TenantId,
                CompanyId = companyId,
                Subject = EntityMap.TryGetString(t, "Subject", "Title", "Name"),
                CreatedAt = EntityMap.TryGetCreatedAt(t),
                LastMessageAt = lastMessageTimes.TryGetValue(t.Id, out var lm) ? lm : null,
                ParticipantsCount = participantCounts.TryGetValue(t.Id, out var pc) ? pc : 0
            }).ToList();

            return Ok(new PagedResult<ThreadDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = dto
            });
        }

        [HttpGet("{threadId:guid}")]
        public async Task<ActionResult<ThreadDetailsDto>> Get(Guid threadId, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            var thread = await _db.MessageThreads.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == threadId && x.TenantId == tenantId && !x.IsDeleted, ct);

            if (thread is null) return NotFound();

            // CompanyId read (entity field adı belirsiz)
            Guid companyId = Guid.Empty;
            try { companyId = EF.Property<Guid>(thread, "CompanyId"); } catch { /* ignore */ }

            var participants = await _db.ThreadParticipants.AsNoTracking()
                .Where(p => p.TenantId == tenantId && !p.IsDeleted)
                .Where(p => EF.Property<Guid>(p, "ThreadId") == threadId)
                .ToListAsync(ct);

            // Kullanıcı katılımcılar: UserId / ParticipantUserId gibi alternatif isimleri deniyoruz.
            var userIds = participants.Select(p =>
            {
                var obj = EntityMap.TryGet(p, "UserId", "ParticipantUserId");
                return Guid.TryParse(obj?.ToString(), out var g) ? g : Guid.Empty;
            })
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

            // Son mesaj zamanı
            var lastAt = await _db.Messages.AsNoTracking()
                .Where(m => m.TenantId == tenantId && !m.IsDeleted && EF.Property<Guid>(m, "ThreadId") == threadId)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => (DateTimeOffset?)m.CreatedAt)
                .FirstOrDefaultAsync(ct);

            return Ok(new ThreadDetailsDto
            {
                Id = thread.Id,
                TenantId = thread.TenantId,
                CompanyId = companyId,
                Subject = EntityMap.TryGetString(thread, "Subject", "Title", "Name"),
                CreatedAt = EntityMap.TryGetCreatedAt(thread),
                LastMessageAt = lastAt,
                ParticipantsCount = participants.Count,
                ParticipantUserIds = userIds
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateThreadRequest req, CancellationToken ct)
        {
            // Neden: Yazışma akışı thread altında toplanır (audit, takip, konu bazlı akış).
            var thread = new MessageThread
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(thread,
                ("CompanyId", req.CompanyId),
                ("Subject", req.Subject),
                ("Title", req.Subject),
                ("Name", req.Subject),
                ("IsActive", true)
            );

            _db.MessageThreads.Add(thread);

            // Katılımcılar (personel)
            foreach (var uid in req.ParticipantUserIds.Distinct())
            {
                var p = new ThreadParticipant
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId,
                    IsDeleted = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                EntityMap.TrySet(p,
                    ("ThreadId", thread.Id),
                    ("MessageThreadId", thread.Id),
                    ("UserId", uid),
                    ("ParticipantUserId", uid),
                    ("Role", 0) // varsa: 0=User varsayımı
                );

                _db.ThreadParticipants.Add(p);
            }

            // İlk mesaj (opsiyonel)
            if (!string.IsNullOrWhiteSpace(req.InitialMessage) && req.InitialSenderUserId is not null)
            {
                var msg = new Message
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId,
                    IsDeleted = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                EntityMap.TrySet(msg,
                    ("ThreadId", thread.Id),
                    ("MessageThreadId", thread.Id),
                    ("SenderUserId", req.InitialSenderUserId),
                    ("UserId", req.InitialSenderUserId),
                    ("Body", req.InitialMessage),
                    ("Text", req.InitialMessage),
                    ("IsInternal", false)
                );

                _db.Messages.Add(msg);
            }

            await _db.SaveChangesAsync(ct);
            return Ok(new { id = thread.Id });
        }
    }
}
