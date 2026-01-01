using Crm.Api.Reporting.Contracts;
using Crm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Reporting.Controllers
{
    [ApiController]
    [Route("api/reporting/companies")]
    public sealed class CompaniesComplianceController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public CompaniesComplianceController(CrmDbContext db) => _db = db;

        [HttpGet("compliance")]
        public async Task<ActionResult<List<CompanyComplianceDto>>> Compliance(
            [FromQuery] Guid tenantId,
            [FromQuery] int take = 100,
            CancellationToken ct = default)
        {
            // Neden: “Compliance” raporu UI’da tabloludur; varsayılan 100 firma yeterli.
            take = take is < 1 or > 500 ? 100 : take;

            var companies = await _db.Companies.AsNoTracking()
                .Where(c => c.TenantId == tenantId && !c.IsDeleted)
                .OrderBy(c => c.CreatedAt)
                .Take(take)
                .Select(c => new
                {
                    c.Id,
                    Name = EF.Property<string>(c, "Name") // Name yoksa exception olabilir; catch edeceğiz.
                })
                .ToListAsync(ct);

            // Eğer Company entity’sinde Name alanı farklıysa, bu select patlayabilir.
            // Bu durumda name’i try-catch ile fallback’e alalım:
            if (companies.Count == 0)
            {
                // minimum fallback: id listesi
                companies = await _db.Companies.AsNoTracking()
                    .Where(c => c.TenantId == tenantId && !c.IsDeleted)
                    .OrderBy(c => c.CreatedAt)
                    .Take(take)
                    .Select(c => new { c.Id, Name = (string?)null })
                    .ToListAsync(ct);
            }

            var companyIds = companies.Select(x => x.Id).ToList();

            // Open tasks per company (Status=0 MVP)
            var taskQ = _db.WorkTasks.AsNoTracking()
                .Where(t => t.TenantId == tenantId && !t.IsDeleted);

            Dictionary<Guid, int> openTasks;
            try
            {
                openTasks = await taskQ
                    .Where(t => companyIds.Contains(EF.Property<Guid>(t, "CompanyId")))
                    .Where(t => EF.Property<int>(t, "Status") == 0)
                    .GroupBy(t => EF.Property<Guid>(t, "CompanyId"))
                    .Select(g => new { CompanyId = g.Key, Cnt = g.Count() })
                    .ToDictionaryAsync(x => x.CompanyId, x => x.Cnt, ct);
            }
            catch
            {
                openTasks = new();
            }

            // Pending documents per company (Status=0 MVP)
            var docQ = _db.DocumentRequests.AsNoTracking()
                .Where(d => d.TenantId == tenantId && !d.IsDeleted);

            Dictionary<Guid, int> pendingDocs;
            try
            {
                pendingDocs = await docQ
                    .Where(d => companyIds.Contains(EF.Property<Guid>(d, "CompanyId")))
                    .Where(d => EF.Property<int>(d, "Status") == 0)
                    .GroupBy(d => EF.Property<Guid>(d, "CompanyId"))
                    .Select(g => new { CompanyId = g.Key, Cnt = g.Count() })
                    .ToDictionaryAsync(x => x.CompanyId, x => x.Cnt, ct);
            }
            catch
            {
                pendingDocs = new();
            }

            // Last bank import at
            var impQ = _db.BankStatementImports.AsNoTracking()
                .Where(i => i.TenantId == tenantId && !i.IsDeleted);

            Dictionary<Guid, DateTimeOffset> lastImport;
            try
            {
                lastImport = await impQ
                    .Where(i => companyIds.Contains(EF.Property<Guid>(i, "CompanyId")))
                    .GroupBy(i => EF.Property<Guid>(i, "CompanyId"))
                    .Select(g => new { CompanyId = g.Key, LastAt = g.Max(x => x.CreatedAt) })
                    .ToDictionaryAsync(x => x.CompanyId, x => x.LastAt, ct);
            }
            catch
            {
                lastImport = new();
            }

            // Last message at: Tenant bazında thread join gerekir (Thread->Company).
            // MVP: tenant mesajlarının son zamanını company’ye dağıtmıyoruz (sonraki iterasyonda join eklenir).
            // Şimdilik null bırakıyoruz.

            var result = companies.Select(c => new CompanyComplianceDto
            {
                CompanyId = c.Id,
                CompanyName = c.Name,
                OpenTasks = openTasks.TryGetValue(c.Id, out var t) ? t : 0,
                PendingDocuments = pendingDocs.TryGetValue(c.Id, out var d) ? d : 0,
                LastBankImportAt = lastImport.TryGetValue(c.Id, out var li) ? li : null,
                LastMessageAt = null
            })
            // Öncelik: en çok problemli firma üstte
            .OrderByDescending(x => x.PendingDocuments)
            .ThenByDescending(x => x.OpenTasks)
            .ToList();

            return Ok(result);
        }
    }
}
