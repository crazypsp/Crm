using Crm.Api.Integration.Contracts;
using Crm.Api.Integration.Security;
using Crm.Data;
using Crm.Services.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Integration.Controllers
{
    [ApiController]
    [Route("api/integration/diagnostics")]
    public sealed class IntegrationDiagnosticsController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly ISecretProtector _protector;

        public IntegrationDiagnosticsController(CrmDbContext db, ISecretProtector protector)
        {
            _db = db;
            _protector = protector;
        }

        [HttpPost("test-connection")]
        public async Task<ActionResult<TestConnectionResultDto>> TestConnection([FromBody] TestConnectionRequest req, CancellationToken ct)
        {
            // 1) Profil + secret doğrula
            var profile = await _db.IntegrationProfiles.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == req.IntegrationProfileId &&
                    x.TenantId == req.TenantId &&
                    !x.IsDeleted &&
                    x.IsActive, ct);

            if (profile is null)
                return Ok(new TestConnectionResultDto { Ok = false, Message = "Profil bulunamadı veya pasif." });

            var secret = await _db.ConnectionSecrets.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == profile.SecretId &&
                    x.TenantId == req.TenantId &&
                    !x.IsDeleted, ct);

            if (secret is null)
                return Ok(new TestConnectionResultDto { Ok = false, Message = "Secret bulunamadı." });

            // 2) Secret çöz
            Dictionary<string, string> settings;
            try
            {
                settings = SecretCrypto.UnprotectSettings(_protector, secret.EncryptedJson);
            }
            catch
            {
                return Ok(new TestConnectionResultDto
                {
                    Ok = false,
                    Message = "Secret çözülemedi (DataProtection key uyumsuzluğu olabilir)."
                });
            }

            // 3) SQL test (Settings içinde varsa)
            if (!settings.TryGetValue("SqlServer", out var server) ||
                !settings.TryGetValue("Database", out var database) ||
                string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(database))
            {
                // Neden: Her entegrasyon SQL olmak zorunda değil (Logo Object, Luca vs.)
                return Ok(new TestConnectionResultDto
                {
                    Ok = true,
                    Message = "SQL parametreleri bulunamadı. Bu ProgramType için detaylı test adapter aşamasında eklenecek."
                });
            }

            // Neden: Derleyiciye kesin atama garantisi vermek için user/pass'i ayrı alıyoruz.
            settings.TryGetValue("User", out var user);
            settings.TryGetValue("Password", out var password);

            var useSqlAuth = !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password);

            var cs = useSqlAuth
                ? $"Server={server};Database={database};User Id={user};Password={password};TrustServerCertificate=True;Encrypt=False;"
                : $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

            try
            {
                await using var conn = new SqlConnection(cs);
                await conn.OpenAsync(ct);

                return Ok(new TestConnectionResultDto
                {
                    Ok = true,
                    Message = "SQL bağlantısı başarılı."
                });
            }
            catch (Exception ex)
            {
                return Ok(new TestConnectionResultDto
                {
                    Ok = false,
                    Message = $"SQL bağlantı hatası: {ex.Message}"
                });
            }
        }
    }
}
