using Crm.Entities.Contracts.Banking;
using Crm.Services.Common;
using UglyToad.PdfPig;

namespace Crm.Services.Banking
{
    public sealed class PdfStatementExtractor : IStatementExtractor
    {
        public Task<ExtractResult> ExtractAsync(Stream file, string fileName, CancellationToken ct)
        {
            try
            {
                using var doc = PdfDocument.Open(file);
                var rows = new List<RawRow>();
                int rowNo = 1;

                foreach (var page in doc.GetPages())
                {
                    var text = page.Text;
                    var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var cells = new Dictionary<string, string?> { ["LINE"] = line.Trim() };
                        rows.Add(new RawRow(rowNo++, cells));
                    }
                }

                return Task.FromResult(new ExtractResult(rows, "PDF", $"PDF: {rows.Count} satır (metin) çıkarıldı. MVP parse gerekebilir."));
            }
            catch (Exception ex)
            {
                throw new ParseException($"PDF okuma hatası: {ex.Message}");
            }
        }
    }
}