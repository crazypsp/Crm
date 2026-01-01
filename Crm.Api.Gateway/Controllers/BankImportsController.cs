using Crm.Api.Gateway.Contracts.Banking;
using Crm.Data;
using Crm.Services.Banking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/bank/imports")]
    public class BankImportsController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly IBankImportAppService _app;

        public BankImportsController(CrmDbContext db, IBankImportAppService app)
        {
            _db = db;
            _app = app;
        }

        // 1) Upload → Import oluştur → Transaction yaz
        [HttpPost]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Upload([FromForm] UploadImportRequest req, CancellationToken ct)
        {
            if (req.File is null || req.File.Length == 0)
                return BadRequest("Dosya boş.");

            await using var stream = req.File.OpenReadStream();

            var importId = await _app.UploadAndCreateImportAsync(
                req.TenantId,
                req.CompanyId,
                req.BankAccountId,
                req.TemplateId,
                stream,
                req.File.FileName,
                req.File.ContentType,
                ct);

            return Ok(new { importId });
        }

        // 2) Import bilgisi (read-model)
        [HttpGet("{importId:guid}")]
        public async Task<ActionResult<BankImportDto>> Get(Guid importId, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            var imp = await _db.BankStatementImports.AsNoTracking()
                .Where(x => x.Id == importId && x.TenantId == tenantId && !x.IsDeleted)
                .Select(x => new BankImportDto
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
                .FirstOrDefaultAsync(ct);

            return imp is null ? NotFound() : Ok(imp);
        }

        // 3) Import içindeki hareketler (preview)
        [HttpGet("{importId:guid}/transactions")]
        public async Task<ActionResult<List<BankTransactionDto>>> GetTransactions(Guid importId, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            var list = await _db.BankTransactions.AsNoTracking()
                .Where(x => x.ImportId == importId && x.TenantId == tenantId && !x.IsDeleted)
                .OrderBy(x => x.RowNo)
                .Select(x => new BankTransactionDto
                {
                    Id = x.Id,
                    RowNo = x.RowNo,
                    TransactionDate = x.TransactionDate,
                    ValueDate = x.ValueDate,
                    Description = x.Description,
                    ReferenceNo = x.ReferenceNo,
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

        // 4) Mapping rules uygula (Business üzerinden)
        [HttpPost("{importId:guid}/apply-mapping")]
        public async Task<IActionResult> ApplyMapping(Guid importId, [FromBody] TenantScopeRequest req, CancellationToken ct)
        {
            await _app.ApplyMappingAsync(req.TenantId, importId, ct);
            return Ok(new { ok = true });
        }

        // 5) Draft üret (Business üzerinden)
        [HttpPost("{importId:guid}/build-draft")]
        public async Task<IActionResult> BuildDraft(Guid importId, [FromBody] BuildDraftRequest req, CancellationToken ct)
        {
            var draftId = await _app.BuildDraftAsync(req.TenantId, importId, req.BankAccountCode, ct);
            return Ok(new { draftId });
        }
    }

    public sealed class UploadImportRequest
    {
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid TemplateId { get; set; }
        public IFormFile File { get; set; } = default!;
    }

    public sealed class TenantScopeRequest
    {
        public Guid TenantId { get; set; }
    }

    public sealed class BuildDraftRequest
    {
        public Guid TenantId { get; set; }
        public string BankAccountCode { get; set; } = default!;
    }
}
