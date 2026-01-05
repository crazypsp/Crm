using Crm.Api.Import.Contracts;
using Crm.Api.Import.Parsing;
using Crm.Api.Import.Storage;
using Crm.Data;
using Crm.Entities.Banking;
using Crm.Entities.Documents;
using Crm.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Import.Controllers;

[ApiController]
[Route("api/import/bank-statement")]
[Authorize]
public sealed class BankStatementImportController : ControllerBase
{
    private readonly CrmDbContext _db;
    private readonly IEnumerable<IBankStatementParser> _parsers;
    private readonly ITempFileStore _temp;
    private readonly IWebHostEnvironment _env;

    public BankStatementImportController(
        CrmDbContext db,
        IEnumerable<IBankStatementParser> parsers,
        ITempFileStore temp,
        IWebHostEnvironment env)
    {
        _db = db;
        _parsers = parsers;
        _temp = temp;
        _env = env;
    }

    /// <summary>
    /// Neden: Dosyayı DB’ye kaydetmeden önce kullanıcıya parse sonuçlarını gösterir.
    /// TempFileId commit aşamasında tekrar kullanılır (tekrar upload yok).
    /// </summary>
    [HttpPost("preview")]
    [RequestSizeLimit(50_000_000)]
    public async Task<ActionResult<PreviewBankStatementResponse>> Preview(
        [FromForm] PreviewBankStatementRequest req,
        [FromForm] IFormFile file,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Dosya boş.");

        if (req.TemplateId == Guid.Empty)
            return BadRequest("TemplateId zorunludur.");

        // Dosyayı temp'e koy
        await using var input = file.OpenReadStream();
        var tempId = await _temp.SaveAsync(input, ct);

        var parser = ResolveParserByFileName(file.FileName);
        if (parser is null)
            return BadRequest("Uygun parser bulunamadı (Excel/PDF).");

        await using var s = await _temp.OpenReadAsync(tempId, ct);
        var rows = await parser.ParseAsync(s, ct);

        return Ok(new PreviewBankStatementResponse
        {
            TempFileId = tempId,
            TotalRows = rows.Count,
            Rows = rows.Take(200).ToList()
        });
    }

    /// <summary>
    /// Neden: Preview’dan gelen temp dosyayı tekrar parse eder ve:
    /// - DocumentFile (source file metadata)
    /// - BankStatementImport (import header)
    /// - BankTransaction (satırlar)
    /// oluşturur.
    /// </summary>
    [HttpPost("commit")]
    public async Task<ActionResult<CommitBankStatementResponse>> Commit(
        [FromBody] CommitBankStatementRequest req,
        CancellationToken ct)
    {
        if (req.TemplateId == Guid.Empty)
            return BadRequest("TemplateId zorunludur.");

        // Tenant sınırında Company var mı?
        var companyExists = await _db.Companies
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == req.TenantId && x.Id == req.CompanyId && !x.IsDeleted, ct);

        if (!companyExists)
            return NotFound("Company bulunamadı.");

        // Tenant sınırında BankAccount var mı?
        var bankExists = await _db.BankAccounts
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == req.TenantId && x.Id == req.BankAccountId && !x.IsDeleted, ct);

        if (!bankExists)
            return NotFound("Bank account bulunamadı.");

        // Template çek (AmountNegativeMeansOutflow kuralı için)
        var template = await _db.BankTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == req.TenantId && x.Id == req.TemplateId && !x.IsDeleted, ct);

        if (template is null)
            return NotFound("Template bulunamadı.");

        // Temp dosyayı aç
        await using var tempStream = await _temp.OpenReadAsync(req.TempFileId, ct);

        // Parser fallback (Excel->PDF)
        var rows = await ParseWithFallbackAsync(tempStream, ct);
        if (rows.Count == 0)
            return BadRequest("Dosya parse edilemedi veya satır bulunamadı.");

        // 1) SourceFileId NOT NULL olduğu için DocumentFile zorunlu.
        var doc = await PersistSourceFileAsync(req.TenantId, req.CompanyId, req.TempFileId, ct);
        _db.DocumentFiles.Add(doc);

        // 2) Import header
        var import = new BankStatementImport
        {
            Id = Guid.NewGuid(),
            TenantId = req.TenantId,
            CompanyId = req.CompanyId,
            BankAccountId = req.BankAccountId,
            TemplateId = req.TemplateId,
            SourceFileId = doc.Id,
            Status = BankImportStatus.Normalized, // enum -> int dönüşümü yok
            TotalRows = rows.Count,
            ImportedRows = 0,
            Notes = null,
            CreatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };

        _db.BankStatementImports.Add(import);

        // 3) Satırları BankTransaction’a yaz
        var imported = 0;

        foreach (var r in rows)
        {
            var signedAmount = ApplySignedAmount(r.Amount, r.Direction, template.AmountNegativeMeansOutflow);

            var tx = new BankTransaction
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                ImportId = import.Id,

                // Entity alanın: TransactionDate (TxDate değil)
                TransactionDate = r.TxDate.ToDateTime(TimeOnly.MinValue),
                ValueDate = null,
                ReferenceNo = null,

                Description = r.Description ?? string.Empty,
                Amount = signedAmount,
                BalanceAfter = r.BalanceAfter,
                RowNo = r.RowNo,

                MappingStatus = MappingStatus.Unmapped,
                SuggestedCounterAccountCode = null,
                ApprovedCounterAccountCode = null,
                AppliedRuleId = null,
                Confidence = null,

                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            _db.BankTransactions.Add(tx);
            imported++;
        }

        import.ImportedRows = imported;

        await _db.SaveChangesAsync(ct);

        // Temp temizle
        await _temp.DeleteAsync(req.TempFileId, ct);

        return Ok(new CommitBankStatementResponse
        {
            ImportId = import.Id,
            ImportedRows = imported
        });
    }

    // ------------------------
    // Helpers
    // ------------------------

    private IBankStatementParser? ResolveParserByFileName(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var desired = ext == ".pdf" ? StatementFileType.Pdf : StatementFileType.Excel;
        return _parsers.FirstOrDefault(p => p.FileType == desired);
    }

    private async Task<IReadOnlyList<PreviewBankStatementRow>> ParseWithFallbackAsync(Stream s, CancellationToken ct)
    {
        // Excel dene
        var excel = _parsers.FirstOrDefault(p => p.FileType == StatementFileType.Excel);
        if (excel is not null)
        {
            try
            {
                if (s.CanSeek) s.Position = 0;
                var rows = await excel.ParseAsync(s, ct);
                if (rows.Count > 0) return rows;
            }
            catch { /* fallback */ }
        }

        // PDF dene
        var pdf = _parsers.FirstOrDefault(p => p.FileType == StatementFileType.Pdf);
        if (pdf is not null)
        {
            try
            {
                if (s.CanSeek) s.Position = 0;
                var rows = await pdf.ParseAsync(s, ct);
                if (rows.Count > 0) return rows;
            }
            catch { /* no-op */ }
        }

        return Array.Empty<PreviewBankStatementRow>();
    }

    private static decimal ApplySignedAmount(decimal amount, MoneyDirection direction, bool negativeMeansOutflow)
    {
        var abs = Math.Abs(amount);

        // negativeMeansOutflow=true => Debit(outflow) negatif, Credit(inflow) pozitif
        if (negativeMeansOutflow)
            return direction == MoneyDirection.Debit ? -abs : abs;

        // tersi
        return direction == MoneyDirection.Debit ? abs : -abs;
    }

    private async Task<DocumentFile> PersistSourceFileAsync(Guid tenantId, Guid companyId, Guid tempFileId, CancellationToken ct)
    {
        // Neden: BankStatementImport.SourceFileId NOT NULL + DocumentFile FK var.
        // Bu yüzden commit sırasında dosyayı storage’a kopyalayıp DocumentFile üretmek zorundayız.

        await using var tempStream = await _temp.OpenReadAsync(tempFileId, ct);

        var now = DateTimeOffset.UtcNow;
        var year = now.Year.ToString();
        var month = now.Month.ToString("00");

        var root = Path.Combine(_env.ContentRootPath, "storage");
        var folder = Path.Combine(
            root,
            "tenants", tenantId.ToString("N"),
            "companies", companyId.ToString("N"),
            "imports", year, month);

        Directory.CreateDirectory(folder);

        var fileName = $"{tempFileId:N}.bin";
        var path = Path.Combine(folder, fileName);

        await using (var fs = System.IO.File.Create(path))
        {
            await tempStream.CopyToAsync(fs, ct);
        }

        return new DocumentFile
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            FileName = fileName,
            ContentType = "application/octet-stream",
            SizeBytes = new FileInfo(path).Length,
            StorageProvider = "local",
            StoragePath = path,
            Sha256 = null,
            CreatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };
    }
}
