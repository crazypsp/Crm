using Crm.Api.Admin.Contracts;
using Crm.Api.Admin.Infrastructure;
using Crm.Data;
using Crm.Entities.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/tenants")]
    public sealed class TenantsController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public TenantsController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<TenantDto>>> List([FromQuery] Guid? dealerId, CancellationToken ct)
        {
            // Neden: Bayi, kendi tenant’larını listelemek ister.
            var q = _db.Tenants.AsNoTracking().Where(x => !x.IsDeleted);

            if (dealerId is not null)
                q = q.Where(t => EF.Property<Guid>(t, "DealerId") == dealerId.Value);

            var tenants = await q.OrderBy(x => x.CreatedAt).ToListAsync(ct);

            var dto = tenants.Select(t => new TenantDto
            {
                Id = t.Id,
                DealerId = SafeGuid(t, "DealerId"),
                Name = EntityMap.TryGetString(t, "Name", "Title")
            }).ToList();

            return Ok(dto);

            static Guid SafeGuid(object entity, string name)
            {
                try
                {
                    // EF.Property sadece query’de, entity instance’da değil; burada reflection ile okuyalım.
                    var v = EntityMap.TryGet(entity, name);
                    return Guid.TryParse(v?.ToString(), out var g) ? g : Guid.Empty;
                }
                catch { return Guid.Empty; }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantRequest req, CancellationToken ct)
        {
            // Neden: Tenant = mali müşavir ofisi.
            // Bayi -> Tenant ilişkisi, çok kiracılı yapının temeli.
            var dealerExists = await _db.Dealers.AsNoTracking()
                .AnyAsync(d => d.Id == req.DealerId && !d.IsDeleted, ct);

            if (!dealerExists) return BadRequest("DealerId bulunamadı.");

            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            EntityMap.TrySet(tenant,
                ("DealerId", req.DealerId),
                ("Name", req.Name),
                ("Title", req.Name),
                ("IsActive", true)
            );

            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = tenant.Id });
        }
    }
}
