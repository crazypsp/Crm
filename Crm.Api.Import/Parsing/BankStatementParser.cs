using Crm.Api.Import.Contracts;

namespace Crm.Api.Import.Parsing
{
    public sealed class BankStatementParser : IBankStatementParser
    {
        private readonly IExcelBankStatementParser _excel;
        private readonly IPdfBankStatementParser _pdf;

        public BankStatementParser(IExcelBankStatementParser excel, IPdfBankStatementParser pdf)
        {
            _excel = excel;
            _pdf = pdf;
        }

        public async Task<PreviewBankStatementResponse> PreviewAsync(IFormFile file, CancellationToken ct)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Neden: Kullanıcı dosyayı excel veya pdf gönderebilir.
            if (ext is ".xls" or ".xlsx") return await _excel.PreviewAsync(file, ct);
            if (ext is ".pdf") return await _pdf.PreviewAsync(file, ct);

            // ContentType fallback
            if ((file.ContentType ?? "").Contains("pdf", StringComparison.OrdinalIgnoreCase))
                return await _pdf.PreviewAsync(file, ct);

            return new PreviewBankStatementResponse
            {
                DetectedFormat = "unknown",
                Warnings = { "Desteklenmeyen dosya tipi (pdf/xls/xlsx bekleniyor)." }
            };
        }
    }
}
