using Crm.Api.Reporting.Contracts;
using Crm.Api.Reporting.Infrastructure;
using Crm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Reporting.Controllers
{
    [ApiController]
    [Route("api/reporting/dashboard")]
    public sealed class DashboardController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public DashboardController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<DashboardSummaryDto>> Get(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid? companyId,
            CancellationToken ct)
        {
            // Neden: Dashboard tek API çağrısıyla KPI + dağılım grafikleri üretir.
            var now = DateTimeOffset.UtcNow;
            var last30 = now.AddDays(-30);
            var last7 = now.AddDays(-7);

            // Companies
            var companiesQ = _db.Companies.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
            {
                // Company bazlı dashboard’da şirket sayısı 1 olmalı, yine de filtreyi güvenli uygula.
                companiesQ = companiesQ.Where(x => x.Id == companyId.Value);
            }

            var companiesCount = await EfTry.SafeCountAsync(companiesQ, ct);

            // WorkTasks (tenant + opsiyonel company)
            var tasksQ = _db.WorkTasks.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
                tasksQ = EfTry.WhereGuidEquals(tasksQ, companyId.Value, "CompanyId");

            // OpenTasksCount: “Status != Done” varsayımı yapmıyoruz.
            // MVP: Status=0 olanları “Open” kabul ediyoruz; istersen status mapping’i sonra standardize ederiz.
            var openTasksQ = EfTry.WhereIntEquals(tasksQ, 0, "Status");
            var openTasksCount = await EfTry.SafeCountAsync(openTasksQ, ct);

            // Tasks by status
            List<StatusCountDto> taskByStatus;
            try
            {
                taskByStatus = await tasksQ
                    .GroupBy(t => EF.Property<int>(t, "Status"))
                    .Select(g => new StatusCountDto { Status = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Status)
                    .ToListAsync(ct);
            }
            catch
            {
                taskByStatus = new();
            }

            // DocumentRequests
            var docReqQ = _db.DocumentRequests.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
                docReqQ = EfTry.WhereGuidEquals(docReqQ, companyId.Value, "CompanyId");

            // PendingDocumentsCount: Status=0 varsayımı (Open/Pending)
            var pendingDocsCount = await EfTry.SafeCountAsync(EfTry.WhereIntEquals(docReqQ, 0, "Status"), ct);

            List<StatusCountDto> docByStatus;
            try
            {
                docByStatus = await docReqQ
                    .GroupBy(d => EF.Property<int>(d, "Status"))
                    .Select(g => new StatusCountDto { Status = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Status)
                    .ToListAsync(ct);
            }
            catch
            {
                docByStatus = new();
            }

            // BankStatementImports
            var importsQ = _db.BankStatementImports.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
                importsQ = EfTry.WhereGuidEquals(importsQ, companyId.Value, "CompanyId");

            var bankImportsCount = await EfTry.SafeCountAsync(importsQ, ct);

            List<StatusCountDto> importsByStatus;
            try
            {
                importsByStatus = await importsQ
                    .GroupBy(i => EF.Property<int>(i, "Status"))
                    .Select(g => new StatusCountDto { Status = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Status)
                    .ToListAsync(ct);
            }
            catch
            {
                importsByStatus = new();
            }

            // BankTransactions last 30 days
            var txQ = _db.BankTransactions.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            if (companyId is not null)
            {
                // BankTransaction doğrudan CompanyId tutmayabilir (Import->Company üzerinden gider).
                // MVP: ImportId üzerinden join yapmak yerine sadece tenant bazlı trend sayısı veriyoruz.
                // İstersen sonraki iterasyonda import join’i ekleriz.
            }

            int txLast30;
            try
            {
                txLast30 = await txQ.Where(x => EF.Property<DateTime>(x, "TransactionDate") >= last30.UtcDateTime).CountAsync(ct);
            }
            catch
            {
                txLast30 = 0;
            }

            // Messages last 7 days
            var msgQ = _db.Messages.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted)
                .Where(x => x.CreatedAt >= last7);

            // Company bazlı mesaj sayımı için thread->company join gerekir; MVP’de tenant bazlı veriyoruz.
            var messagesLast7 = await EfTry.SafeCountAsync(msgQ, ct);

            return Ok(new DashboardSummaryDto
            {
                TenantId = tenantId,
                CompanyId = companyId,
                CompaniesCount = companiesCount,
                OpenTasksCount = openTasksCount,
                PendingDocumentsCount = pendingDocsCount,
                BankImportsCount = bankImportsCount,
                BankTransactionsLast30Days = txLast30,
                MessagesLast7Days = messagesLast7,
                TaskByStatus = taskByStatus,
                DocumentRequestsByStatus = docByStatus,
                BankImportsByStatus = importsByStatus
            });
        }
    }
}
