using Crm.Api.Messaging.Contracts;
using Crm.Api.Messaging.Infrastructure;
using Crm.Data;
using Crm.Entities.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Messaging.Controllers
{
    [ApiController]
    [Route("api/messaging/participants")]
    public sealed class ParticipantsController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public ParticipantsController(CrmDbContext db) => _db = db;

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddParticipantsRequest req, CancellationToken ct)
        {
            var threadExists = await _db.MessageThreads.AsNoTracking()
                .AnyAsync(t => t.Id == req.ThreadId && t.TenantId == req.TenantId && !t.IsDeleted, ct);

            if (!threadExists) return NotFound("Thread bulunamadı.");

            foreach (var uid in req.UserIds.Distinct())
            {
                var p = new ThreadParticipant
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId,
                    IsDeleted = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                EntityMap.TrySet(p,
                    ("ThreadId", req.ThreadId),
                    ("MessageThreadId", req.ThreadId),
                    ("UserId", uid),
                    ("ParticipantUserId", uid),
                    ("Role", 0)
                );

                _db.ThreadParticipants.Add(p);
            }

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> Remove(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid threadId,
            [FromQuery] Guid userId,
            CancellationToken ct)
        {
            // Neden: Soft delete ile audit korunur.
            var candidates = await _db.ThreadParticipants
                .Where(p => p.TenantId == tenantId && !p.IsDeleted)
                .ToListAsync(ct);

            var match = candidates.FirstOrDefault(p =>
            {
                var tidObj = EntityMap.TryGet(p, "ThreadId", "MessageThreadId");
                var uidObj = EntityMap.TryGet(p, "UserId", "ParticipantUserId");

                var tidOk = Guid.TryParse(tidObj?.ToString(), out var tid) && tid == threadId;
                var uidOk = Guid.TryParse(uidObj?.ToString(), out var uid) && uid == userId;

                return tidOk && uidOk;
            });

            if (match is null) return NotFound("Katılımcı bulunamadı.");

            match.IsDeleted = true;
            match.DeletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }
    }
}
