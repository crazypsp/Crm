using Crm.Api.Import.Contracts;
using Crm.Api.Import.Parsing;
using Crm.Api.Import.Storage;
using Crm.Data;
using Crm.Entities.Banking;
using Crm.Entities.Documents;
using Crm.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Api.Import.Controllers
{
    [ApiController]
    [Route("api/import/bank-statements")]
    public sealed class BankStatementImportController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly IBankStatementParser _parser;
        private readonly IImportFileStorage _storage;

        public BankStatementImportController(CrmDbContext db, IBankStatementParser parser, IImportFileStorage storage)
        {
            _db = db;
            _parser = parser;
            _storage = storage;
        }

        [HttpPost("preview")]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<PreviewBankStatementResponse>> Preview([FromForm] IFormFile file, CancellationToken ct)
        {
            // Neden: DB’ye yazmadan önce kullanıcı satırları görüp doğrulasın.
            return Ok(await _parser.PreviewAsync(file, ct));
        }

        [HttpPost("commit")]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<CommitBankStatementResponse>> Commit(
            [FromForm] Guid tenantId,
            [FromForm] Guid companyId,
            [FromForm] Guid bankAccountId,
            [FromForm] Guid templateId,
            [FromForm] int? year,
            [FromForm] int? month,
            [FromForm] IFormFile file,
            CancellationToken ct)
        {
            // 1) Parse
            var preview = await _parser.PreviewAsync(file, ct);
            if (preview.Rows.Count == 0)
                return BadRequest(new { message = "Import edilecek satır bulunamadı.", preview.Warnings });

            // 2) Period (klasörleme)
            var period = ResolvePeriod(preview, year, month);

            // 3) Dosyayı Documents standardında kaydet
            var saved = await _storage.SaveAsync(tenantId, companyId, file, period, ct);

            // 4) Ham dosyayı DB’de izleyebilmek için DocumentFile kaydı
            // Neden: Hem import dosyasını arayabilmek hem de audit için.
            var doc = new DocumentFile
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            // Not: DocumentFile entity’nizde alan isimleri farklıysa burayı entity’nize göre uyarlayın.
            // Aşağıdaki alanlar Documents projesindeki standarda uygun beklenen alanlardır.
            doc.GetType().GetProperty("CompanyId")?.SetValue(doc, companyId);
            doc.GetType().GetProperty("OriginalFileName")?.SetValue(doc, saved.OriginalFileName);
            doc.GetType().GetProperty("ContentType")?.SetValue(doc, saved.ContentType);
            doc.GetType().GetProperty("SizeBytes")?.SetValue(doc, saved.SizeBytes);
            doc.GetType().GetProperty("StoragePath")?.SetValue(doc, saved.RelativePath);
            doc.GetType().GetProperty("Sha256")?.SetValue(doc, saved.Sha256);

            _db.DocumentFiles.Add(doc);

            // 5) Import header kaydı
            // Neden: Import işleminin “tek kimliği” olur. Banking burada üretilen ImportId ile çalışır.
            var import = new BankStatementImport
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CompanyId = companyId,
                BankAccountId = bankAccountId,
                TemplateId = templateId,
                SourceFileId = doc.Id,
                Status = BankImportStatus.Uploaded, // MVP: 1 = Imported (enum’unuz varsa ona göre düzeltin)
                TotalRows = preview.Rows.Count,
                ImportedRows = preview.Rows.Count,
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            _db.BankStatementImports.Add(import);

            // 6) Transaction satırlarını yaz
            // Neden: Normalized satırlar muhasebe/fiş üretiminin temel girdisidir.
            foreach (var r in preview.Rows)
            {
                var amountSigned = r.Debit > 0 ? -r.Debit : r.Credit;

                var tx = new BankTransaction
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    ImportId = import.Id,
                    RowNo = r.RowNo,
                    TransactionDate = r.TransactionDate,
                    Description = r.Description,
                    Amount = amountSigned,
                    BalanceAfter = r.Balance,
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };

                _db.BankTransactions.Add(tx);
            }

            await _db.SaveChangesAsync(ct);

            return Ok(new CommitBankStatementResponse
            {
                ImportId = import.Id,
                TotalRows = preview.Rows.Count,
                ImportedRows = preview.Rows.Count
            });
        }

        private static DateTime ResolvePeriod(PreviewBankStatementResponse preview, int? year, int? month)
        {
            // Neden: Kullanıcı dönem seçerse klasörlemeyi kontrol eder.
            if (year is not null && month is not null)
            {
                if (month < 1 || month > 12)
                    throw new ArgumentOutOfRangeException(nameof(month), "month 1..12 olmalı");

                // Doğru overload: (year, month, day)  -> DateTimeKind hatası buradan gelmez.
                return new DateTime(year.Value, month.Value, 1);
            }

            // Yoksa ilk işlem tarihine göre klasörle
            var first = preview.Rows.OrderBy(x => x.TransactionDate).First().TransactionDate;
            return new DateTime(first.Year, first.Month, 1);
        }
    }
}
