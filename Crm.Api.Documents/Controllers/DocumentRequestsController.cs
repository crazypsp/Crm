using Crm.Api.Documents.Contracts;
using Crm.Api.Documents.Infrastructure;
using Crm.Data;
using Crm.Entities.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Documents.Controllers
{
    [ApiController]
    [Route("api/documents/requests")]
    public sealed class DocumentRequestsController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public DocumentRequestsController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<DocumentRequestDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid companyId,
            CancellationToken ct)
        {
            // Neden: Firma ekranında “evrak talepleri” listesi görünür.
            var requests = await _db.DocumentRequests.AsNoTracking()
                .Where(x => x.TenantId == tenantId &&
                            EF.Property<Guid>(x, "CompanyId") == companyId &&
                            !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            // Neden: Item sayısı UI’da “kaç kalem var?” göstermek için.
            var requestIds = requests.Select(r => r.Id).ToList();

            var itemCounts = await _db.DocumentRequestItems.AsNoTracking()
                .Where(i => i.TenantId == tenantId &&
                            requestIds.Contains(EF.Property<Guid>(i, "RequestId")) &&
                            !i.IsDeleted)
                .GroupBy(i => EF.Property<Guid>(i, "RequestId"))
                .Select(g => new { RequestId = g.Key, Cnt = g.Count() })
                .ToDictionaryAsync(x => x.RequestId, x => x.Cnt, ct);

            var dto = requests.Select(r => new DocumentRequestDto
            {
                Id = r.Id,
                TenantId = r.TenantId,
                CompanyId = companyId,
                Title = EntityMap.TryGetString(r, "Title", "Name", "Subject"),
                Notes = EntityMap.TryGetString(r, "Notes", "Description"),
                DueDate = EntityMap.TryGetDateTimeOffset(r, "DueDate", "Deadline"),
                Status = EntityMap.TryGetInt(r, "Status"),
                ItemsCount = itemCounts.TryGetValue(r.Id, out var c) ? c : 0,
                CreatedAt = EntityMap.TryGetCreatedAt(r)
            }).ToList();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentRequestRequest req, CancellationToken ct)
        {
            // Neden: Evrak talebi (request) firma/personelden istenecek doküman setinin “başlık” kaydıdır.
            var request = new DocumentRequest
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(request,
                ("CompanyId", req.CompanyId),
                ("Title", req.Title),
                ("Name", req.Title),
                ("Subject", req.Title),
                ("Notes", req.Notes),
                ("Description", req.Notes),
                ("DueDate", req.DueDate),
                ("Deadline", req.DueDate),
                ("Status", 0) // 0=open varsayımı
            );

            _db.DocumentRequests.Add(request);

            foreach (var item in req.Items)
            {
                var it = new DocumentRequestItem
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId,
                    IsDeleted = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                EntityMap.TrySet(it,
                    ("RequestId", request.Id),
                    ("Name", item.Name),
                    ("Title", item.Name),
                    ("IsRequired", item.IsRequired),
                    ("Required", item.IsRequired),
                    ("Status", 0)
                );

                _db.DocumentRequestItems.Add(it);
            }

            await _db.SaveChangesAsync(ct);

            return Ok(new { id = request.Id });
        }
    }
}
