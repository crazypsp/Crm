using Crm.Api.Admin.Contracts;
using Crm.Api.Admin.Infrastructure;
using Crm.Data;
using Crm.Entities.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/companies")]
    public sealed class CompaniesController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public CompaniesController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> List([FromQuery] Guid tenantId, CancellationToken ct)
        {
            // Neden: Mali müşavir, kendi firmalarını (mükelleflerini) listeler.
            var companies = await _db.Companies.AsNoTracking()
                .Where(c => c.TenantId == tenantId && !c.IsDeleted)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(ct);

            var dto = companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                TenantId = c.TenantId,
                Name = EntityMap.TryGetString(c, "Name", "Title"),
                TaxNo = EntityMap.TryGetString(c, "TaxNo", "TaxNumber", "VknTckn")
            }).ToList();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyRequest req, CancellationToken ct)
        {
            var tenantExists = await _db.Tenants.AsNoTracking()
                .AnyAsync(t => t.Id == req.TenantId && !t.IsDeleted, ct);

            if (!tenantExists) return BadRequest("TenantId bulunamadı.");

            var company = new Company
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            EntityMap.TrySet(company,
                ("Name", req.Name),
                ("Title", req.Name),
                ("TaxNo", req.TaxNo),
                ("TaxNumber", req.TaxNo),
                ("VknTckn", req.TaxNo),
                ("IsActive", true)
            );

            _db.Companies.Add(company);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = company.Id });
        }
    }
}
