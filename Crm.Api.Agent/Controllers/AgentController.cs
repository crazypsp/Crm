using System.Security.Cryptography;
using System.Text;
using Crm.Data;
using Crm.Entities.Integration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Agent.Controllers
{
    [ApiController]
    [Route("api/agent")]
    public sealed class AgentController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public AgentController(CrmDbContext db) => _db = db;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAgentRequest req, CancellationToken ct)
        {
            // Bu endpoint middleware tarafından X-Registration-Key ile korunuyor.
            // Neden: İlk agent kaydı kontrollü olmalı.

            // 1) AgentKey üret (plaintext) — sadece bu response ile agent’a verilir.
            // Neden: Agent daha sonra tüm isteklerinde bu key’i header ile gönderecek.
            var agentKeyPlain = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var agentKeyHash = Sha256(agentKeyPlain);

            // 2) AgentMachine kaydı oluştur
            // TODO: Alan adları sende farklıysa uyarlayacaksın.
            var entity = new AgentMachine
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                MachineName = req.MachineName,
                AgentVersion = req.Version,

                IsOnline = true,
                IsDeleted = false,

                CreatedAt = DateTimeOffset.UtcNow,
                LastSeenAt = DateTimeOffset.UtcNow
            };

            _db.AgentMachines.Add(entity);
            await _db.SaveChangesAsync(ct);

            // 3) AgentKey’i döndür (1 kere)
            // Neden: DB’de sadece hash tutulur; plaintext saklanmaz.
            return Ok(new
            {
                agentMachineId = entity.Id,
                agentKey = agentKeyPlain
            });
        }

        [HttpPost("heartbeat")]
        public async Task<IActionResult> Heartbeat(CancellationToken ct)
        {
            // Neden: Agent’ın online/offline takibi ve health monitoring için.
            var agentMachineId = (Guid)HttpContext.Items["AgentMachineId"]!;

            var agent = await _db.AgentMachines.FirstOrDefaultAsync(x => x.Id == agentMachineId, ct);
            if (agent is null) return NotFound();

            agent.LastSeenAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);

            return Ok(new { ok = true, agentMachineId });
        }

        private static string Sha256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }

    public sealed class RegisterAgentRequest
    {
        // Neden: Agent belirli bir mali müşavir ofisi (tenant) adına çalışır.
        public Guid TenantId { get; set; }

        // Neden: UI tarafında cihaz listesi ve sorun tespiti için.
        public string MachineName { get; set; } = default!;

        // Neden: Agent sürüm uyumluluğu ve rollout yönetimi için.
        public string Version { get; set; } = "1.0.0";
    }
}
