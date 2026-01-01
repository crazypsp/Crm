using Crm.Api.Banking.Contracts;
using Crm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Banking.Controllers
{
    [ApiController]
    [Route("api/banking/imports")]
    public sealed class ImportsQueryController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public ImportsQueryController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<PagedResult<BankImportListItemDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid companyId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            // Neden: Import listesi büyüyebilir; sayfalama zorunludur.
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;

            var baseQuery = _db.BankStatementImports.AsNoTracking()
                .Where(x => x.TenantId == tenantId && x.CompanyId == companyId && !x.IsDeleted);

            var total = await baseQuery.CountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BankImportListItemDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    BankAccountId = x.BankAccountId,
                    TemplateId = x.TemplateId,
                    SourceFileId = x.SourceFileId,
                    Status = (int)x.Status,
                    TotalRows = x.TotalRows,
                    ImportedRows = x.ImportedRows,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

            return Ok(new PagedResult<BankImportListItemDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            });
        }

        [HttpGet("{importId:guid}/transactions")]
        public async Task<ActionResult<List<BankTransactionListItemDto>>> Transactions(
            Guid importId,
            [FromQuery] Guid tenantId,
            CancellationToken ct)
        {
            // Neden: UI import detay ekranında satırları gösterir.
            var list = await _db.BankTransactions.AsNoTracking()
                .Where(x => x.TenantId == tenantId && x.ImportId == importId && !x.IsDeleted)
                .OrderBy(x => x.RowNo)
                .Select(x => new BankTransactionListItemDto
                {
                    Id = x.Id,
                    RowNo = x.RowNo,
                    TransactionDate = x.TransactionDate,
                    Description = x.Description,
                    Amount = x.Amount,
                    BalanceAfter = x.BalanceAfter,
                    MappingStatus = (int)x.MappingStatus,
                    SuggestedCounterAccountCode = x.SuggestedCounterAccountCode,
                    ApprovedCounterAccountCode = x.ApprovedCounterAccountCode,
                    Confidence = x.Confidence
                })
                .ToListAsync(ct);

            return Ok(list);
        }
    }
}
