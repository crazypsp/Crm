using Crm.Business.Banking;
using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Documents;
using Crm.Services.Common;
using Crm.Services.Storage;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Crm.Services.Banking
{
    public sealed class BankImportAppService : IBankImportAppService
    {
        private readonly CrmDbContext _db;
        private readonly IBankImportManager _manager;
        private readonly IFileStorage _storage;
        private readonly IStatementExtractor _excelExtractor;
        private readonly IStatementExtractor _pdfExtractor;
        private readonly IBankStatementNormalizer _normalizer;
        private readonly IClock _clock;

        public BankImportAppService(
            CrmDbContext db,
            IBankImportManager manager,
            IFileStorage storage,
            ExcelStatementExtractor excelExtractor,
            PdfStatementExtractor pdfExtractor,
            IBankStatementNormalizer normalizer,
            IClock clock)
        {
            _db = db;
            _manager = manager;
            _storage = storage;
            _excelExtractor = excelExtractor;
            _pdfExtractor = pdfExtractor;
            _normalizer = normalizer;
            _clock = clock;
        }

        public async Task<Guid> UploadAndCreateImportAsync(
            Guid tenantId,
            Guid companyId,
            Guid bankAccountId,
            Guid templateId,
            Stream file,
            string fileName,
            string contentType,
            CancellationToken ct)
        {
            // 1) Dosyayı storage’a yaz (IO işi)
            var storagePath = await _storage.SaveAsync(file, fileName, ct);

            // 2) DocumentFile metadata kaydı oluştur (DB’ye yazma burada; çünkü file metadata altyapı parçası)
            var doc = new DocumentFile
            {
                TenantId = tenantId,
                FileName = Path.GetFileName(fileName),
                ContentType = contentType,
                SizeBytes = 0, // Stream length her zaman alınamayabilir; istersen burada hesaplanır.
                StorageProvider = "local",
                StoragePath = storagePath,
                CreatedAt = _clock.UtcNow
            };
            _db.DocumentFiles.Add(doc);
            await _db.SaveChangesAsync(ct);

            // 3) Business: import oturumu aç
            var import = await _manager.CreateImportAsync(tenantId, companyId, bankAccountId, templateId, doc.Id, ct);

            // 4) Template’i DB’den çek (ColumnMapJson için)
            var template = await _db.BankTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == templateId && x.TenantId == tenantId, ct);

            if (template is null)
                throw new NotFoundException("Banka şablonu bulunamadı.");

            var map = JsonSerializer.Deserialize<Dictionary<string, string>>(template.ColumnMapJson)
                      ?? throw new ParseException("ColumnMapJson parse edilemedi.");

            // 5) Dosyayı tekrar aç ve extract et (format seçimi)
            await using var readStream = await _storage.OpenReadAsync(storagePath, ct);

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            var extractor = ext switch
            {
                ".xlsx" or ".xls" => _excelExtractor,
                ".pdf" => _pdfExtractor,
                _ => throw new UnsupportedFileException("Sadece Excel (.xlsx/.xls) veya PDF desteklenir.")
            };

            var extract = await extractor.ExtractAsync(readStream, fileName, ct);

            // 6) Normalize (Excel için)
            // PDF için normalize başka bir adım ister; MVP’de PDF extractor “LINE” döndürüyor.
            if (ext is ".pdf")
                throw new ParseException("PDF normalize MVP’de temel. Bu banka PDF formatına göre regex/tablolaştırma eklemek gerekir.");

            var normalized = _normalizer.NormalizeExcelRows(tenantId, import.Id, map, extract.Rows);

            // 7) Business: transaction’ları DB’ye yaz
            await _manager.AddTransactionsAsync(tenantId, import.Id, normalized, ct);

            return import.Id;
        }

        public Task ApplyMappingAsync(Guid tenantId, Guid importId, CancellationToken ct)
            => _manager.ApplyMappingRulesAsync(tenantId, importId, ct);

        public async Task<Guid> BuildDraftAsync(Guid tenantId, Guid importId, string bankAccountCode, CancellationToken ct)
        {
            var draft = await _manager.BuildVoucherDraftAsync(tenantId, importId, bankAccountCode, ct);
            return draft.Id;
        }
    }
}
