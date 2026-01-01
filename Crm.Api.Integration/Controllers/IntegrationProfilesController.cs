using Crm.Api.Integration.Contracts;
using Crm.Api.Integration.Security;
using Crm.Data;
using Crm.Entities.Integration;
using Crm.Services.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Crm.Api.Integration.Controllers
{

    [ApiController]
    [Route("api/integration/profiles")]
    public sealed class IntegrationProfilesController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly ISecretProtector _protector;

        public IntegrationProfilesController(CrmDbContext db, ISecretProtector protector)
        {
            _db = db;
            _protector = protector;
        }

        [HttpGet]
        public async Task<ActionResult<List<IntegrationProfileDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid? companyId,
            CancellationToken ct)
        {
            // Neden: UI profil seçimi için liste ister. Secret içerik güvenlik nedeniyle dönülmez.
            var q = _db.IntegrationProfiles.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
                q = q.Where(x => x.CompanyId == companyId);

            var list = await q
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new IntegrationProfileDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    CompanyId = x.CompanyId,
                    ProgramType = x.ProgramType,
                    SecretId = x.SecretId,
                    BranchCode = x.BranchCode,
                    WorkplaceCode = x.WorkplaceCode,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IntegrationProfileDto>> Get(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            var dto = await _db.IntegrationProfiles.AsNoTracking()
                .Where(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted)
                .Select(x => new IntegrationProfileDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    CompanyId = x.CompanyId,
                    ProgramType = x.ProgramType,
                    SecretId = x.SecretId,
                    BranchCode = x.BranchCode,
                    WorkplaceCode = x.WorkplaceCode,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync(ct);

            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpsertIntegrationProfileRequest req, CancellationToken ct)
        {
            // Neden: IntegrationProfile.SecretId zorunlu. Profil secretsiz anlamsız.
            var secret = new ConnectionSecret
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                EncryptedJson = SecretCrypto.ProtectSettings(_protector, req.Settings),
                Notes = null,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var profile = new IntegrationProfile
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                CompanyId = req.CompanyId,
                ProgramType = req.ProgramType,

                SecretId = secret.Id,
                Secret = secret, // Neden: aynı transaction’da insert kolaylığı.

                BranchCode = req.BranchCode,
                WorkplaceCode = req.WorkplaceCode,
                IsActive = req.IsActive,

                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.ConnectionSecrets.Add(secret);
            _db.IntegrationProfiles.Add(profile);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = profile.Id, secretId = secret.Id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertIntegrationProfileRequest req, CancellationToken ct)
        {
            var profile = await _db.IntegrationProfiles
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == req.TenantId && !x.IsDeleted, ct);

            if (profile is null) return NotFound();

            profile.CompanyId = req.CompanyId;
            profile.ProgramType = req.ProgramType;
            profile.BranchCode = req.BranchCode;
            profile.WorkplaceCode = req.WorkplaceCode;
            profile.IsActive = req.IsActive;
            profile.UpdatedAt = DateTimeOffset.UtcNow;

            // Neden: Secret profilin ayrılmaz parçası; güncellemede de encrypt edilerek yazılır.
            var secret = await _db.ConnectionSecrets
                .FirstOrDefaultAsync(x => x.Id == profile.SecretId && x.TenantId == req.TenantId && !x.IsDeleted, ct);

            if (secret is null) return Problem("Profile bağlı secret bulunamadı. Veri bütünlüğü kontrol edilmeli.");

            secret.EncryptedJson = SecretCrypto.ProtectSettings(_protector, req.Settings);
            secret.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            // Neden: Soft delete audit için önemli.
            var profile = await _db.IntegrationProfiles
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted, ct);

            if (profile is null) return NotFound();

            profile.IsDeleted = true;
            profile.DeletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }
    }
}
