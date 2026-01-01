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
    [Route("api/banking/rules")]
    public sealed class BankMappingRulesController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public BankMappingRulesController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<BankMappingRuleDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid? companyId,
            [FromQuery] Guid? templateId,
            CancellationToken ct)
        {
            // Neden: Rule yönetimi ekranında filtreli liste gerekir.
            var query = _db.BankMappingRules.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
                query = query.Where(x => EF.Property<Guid?>(x, "CompanyId") == companyId);

            if (templateId is not null)
                query = query.Where(x => EF.Property<Guid?>(x, "TemplateId") == templateId);

            var entities = await query.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

            var list = entities.Select(e => new BankMappingRuleDto
            {
                Id = e.Id,
                TenantId = e.TenantId,
                CompanyId = EntityMap.TryGetGuid(e, "CompanyId"),
                TemplateId = EntityMap.TryGetGuid(e, "TemplateId"),
                MatchText = EntityMap.TryGetString(e, "MatchText", "ContainsText", "Keyword", "DescriptionContains"),
                CounterAccountCode = EntityMap.TryGetString(e, "CounterAccountCode", "CounterCode", "AccountingCounterAccountCode"),
                Confidence = TryDecimal(EntityMap.TryGet(e, "Confidence", "Score")),
                Priority = TryInt(EntityMap.TryGet(e, "Priority", "Order")),
                IsActive = EntityMap.TryGetBool(e, "IsActive"),
                CreatedAt = EntityMap.TryGetCreatedAt(e)
            }).ToList();

            return Ok(list);

            static decimal? TryDecimal(object? v)
                => v is null ? null : (decimal.TryParse(v.ToString(), out var d) ? d : null);

            static int? TryInt(object? v)
                => v is null ? null : (int.TryParse(v.ToString(), out var i) ? i : null);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpsertBankMappingRuleRequest req, CancellationToken ct)
        {
            // Neden: Banka hareket açıklamasından karşı hesap önerisi üretmek otomasyonu sağlar.
            var entity = new BankMappingRule
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(entity,
                ("CompanyId", req.CompanyId),
                ("TemplateId", req.TemplateId),
                ("MatchText", req.MatchText),
                ("ContainsText", req.MatchText),
                ("Keyword", req.MatchText),
                ("CounterAccountCode", req.CounterAccountCode),
                ("AccountingCounterAccountCode", req.CounterAccountCode),
                ("Confidence", req.Confidence),
                ("Score", req.Confidence),
                ("Priority", req.Priority),
                ("Order", req.Priority),
                ("IsActive", req.IsActive)
            );

            _db.BankMappingRules.Add(entity);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = entity.Id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertBankMappingRuleRequest req, CancellationToken ct)
        {
            var entity = await _db.BankMappingRules.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == req.TenantId && !x.IsDeleted, ct);
            if (entity is null) return NotFound();

            EntityMap.TrySet(entity,
                ("CompanyId", req.CompanyId),
                ("TemplateId", req.TemplateId),
                ("MatchText", req.MatchText),
                ("ContainsText", req.MatchText),
                ("Keyword", req.MatchText),
                ("CounterAccountCode", req.CounterAccountCode),
                ("AccountingCounterAccountCode", req.CounterAccountCode),
                ("Confidence", req.Confidence),
                ("Score", req.Confidence),
                ("Priority", req.Priority),
                ("Order", req.Priority),
                ("IsActive", req.IsActive),
                ("UpdatedAt", DateTimeOffset.UtcNow)
            );

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            // Neden: Rule değişiklikleri muhasebe otomasyonunu etkiler; soft delete audit için iyidir.
            var entity = await _db.BankMappingRules.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted, ct);
            if (entity is null) return NotFound();

            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }
    }
}
