using Crm.Api.Banking.Contracts;
using Crm.Api.Banking.Infrastructure;
using Crm.Data;
using Crm.Entities.Banking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Banking.Controllers
{
    [ApiController]
    [Route("api/banking/templates")]
    public sealed class BankTemplatesController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public BankTemplatesController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<BankTemplateDto>>> List([FromQuery] Guid tenantId, [FromQuery] Guid? companyId, CancellationToken ct)
        {
            // Neden: Template yönetimi ekranında liste gerekir; soft delete filtrelenir.
            var query = _db.BankTemplates.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            // CompanyId entity’de yoksa bile query derlenir (çünkü burada entity property’sine dokunmuyoruz).
            // Ancak çoğu yapıda companyId vardır; yoksa bu filtreyi kaldırırsın.
            if (companyId is not null)
                query = query.Where(x => EF.Property<Guid?>(x, "CompanyId") == companyId);

            var entities = await query.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

            var list = entities.Select(e => new BankTemplateDto
            {
                Id = e.Id,
                TenantId = e.TenantId,
                CompanyId = EntityMap.TryGetGuid(e, "CompanyId"),
                Name = EntityMap.TryGetString(e, "Name", "Title"),
                BankName = EntityMap.TryGetString(e, "BankName"),
                IsActive = EntityMap.TryGetBool(e, "IsActive"),
                CreatedAt = EntityMap.TryGetCreatedAt(e)
            }).ToList();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpsertBankTemplateRequest req, CancellationToken ct)
        {
            // Neden: Banka dosyası okuma (PDF/Excel) bankaya göre değişir; bu değişkenliği template ile yönetiriz.
            var entity = new BankTemplate
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            // Neden: Entity alanları sende farklı olabilir; güvenli set.
            EntityMap.TrySet(entity,
                ("CompanyId", req.CompanyId),
                ("Name", req.Name),
                ("Title", req.Name),
                ("BankName", req.BankName),
                ("IsActive", req.IsActive),
                // Template JSON alanı sende nasıl adlandıysa: DefinitionJson / TemplateJson / SettingsJson vb.
                ("DefinitionJson", EntityMap.SerializeDefinition(req.Definition)),
                ("TemplateJson", EntityMap.SerializeDefinition(req.Definition)),
                ("SettingsJson", EntityMap.SerializeDefinition(req.Definition))
            );

            _db.BankTemplates.Add(entity);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = entity.Id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertBankTemplateRequest req, CancellationToken ct)
        {
            var entity = await _db.BankTemplates.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == req.TenantId && !x.IsDeleted, ct);
            if (entity is null) return NotFound();

            EntityMap.TrySet(entity,
                ("CompanyId", req.CompanyId),
                ("Name", req.Name),
                ("Title", req.Name),
                ("BankName", req.BankName),
                ("IsActive", req.IsActive),
                ("DefinitionJson", EntityMap.SerializeDefinition(req.Definition)),
                ("TemplateJson", EntityMap.SerializeDefinition(req.Definition)),
                ("SettingsJson", EntityMap.SerializeDefinition(req.Definition)),
                ("UpdatedAt", DateTimeOffset.UtcNow)
            );

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            // Neden: Template geçmişi/audit için soft delete.
            var entity = await _db.BankTemplates.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted, ct);
            if (entity is null) return NotFound();

            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }
    }
}
