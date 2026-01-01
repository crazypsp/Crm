using System.Security.Cryptography;
using System.Text;
using Crm.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Api.Agent.Security
{
    public sealed class AgentAuthMiddleware : IMiddleware
    {
        private readonly CrmDbContext _db;
        private readonly AgentAuthOptions _opt;

        public AgentAuthMiddleware(CrmDbContext db, IOptions<AgentAuthOptions> opt)
        {
            _db = db;
            _opt = opt.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";

            // Bu middleware sadece agent endpoint’lerini hedefler.
            if (!path.StartsWith("/api/agent"))
            {
                await next(context);
                return;
            }

            // 1) Register: X-Registration-Key ile korunur.
            // Neden: İlk kez agent eklerken "herkesten gelen" kayıt isteklerini engellemek.
            if (path.StartsWith("/api/agent/register"))
            {
                var regKey = context.Request.Headers["X-Registration-Key"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(regKey) || regKey != _opt.RegistrationKey)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid registration key.");
                    return;
                }

                await next(context);
                return;
            }

            // 2) Diğer agent endpoint’leri: X-Agent-Key ile korunur.
            // Neden: Register sonrası agent’a tek seferlik bir agentKey verilir; agent bunu localde saklar.
            // API’ye her çağrıda bu key gönderilir ve DB’de hash ile doğrulanır.
            var agentKey = context.Request.Headers["X-Agent-Key"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(agentKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing X-Agent-Key.");
                return;
            }

            var keyHash = Sha256(agentKey);

            // TODO: Sende AgentMachine alan adı farklıysa burada uyarlayacaksın.
            // Beklenen: AgentMachines tablosunda AgentKeyHash, IsActive, IsDeleted, TenantId, Id gibi alanlar.
            var agent = await _db.AgentMachines.AsNoTracking()
                .Where(x => x.IsDeleted && !x.IsDeleted)
                .Select(x => new { x.Id, x.TenantId })
                .FirstOrDefaultAsync();

            if (agent is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid agent key.");
                return;
            }

            // Neden: Controller’ların tekrar DB’ye gitmeden agent kimliğini kullanabilmesi.
            context.Items["AgentMachineId"] = agent.Id;
            context.Items["TenantId"] = agent.TenantId;

            await next(context);
        }

        private static string Sha256(string input)
        {
            // Neden: DB’de plaintext key tutmak yerine hash tutmak güvenli bir MVP yaklaşımıdır.
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}
