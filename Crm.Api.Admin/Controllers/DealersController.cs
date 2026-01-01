using Crm.Api.Admin.Contracts;
using Crm.Api.Admin.Infrastructure;
using Crm.Data;
using Crm.Entities.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/dealers")]
    public sealed class DealersController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public DealersController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<DealerDto>>> List(CancellationToken ct)
        {
            // Neden: Admin ekranında bayi listesi.
            var dealers = await _db.Dealers.AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(ct);

            var dto = dealers.Select(d => new DealerDto
            {
                Id = d.Id,
                Name = EntityMap.TryGetString(d, "Name", "Title")
            }).ToList();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDealerRequest req, CancellationToken ct)
        {
            // Neden: Bayi = mali müşavirleri yöneten üst yapı.
            var dealer = new Dealer
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            EntityMap.TrySet(dealer,
                ("Name", req.Name),
                ("Title", req.Name),
                ("Notes", req.Notes),
                ("IsActive", true)
            );

            _db.Dealers.Add(dealer);
            await _db.SaveChangesAsync(ct);

            return Ok(new { id = dealer.Id });
        }
    }
}
