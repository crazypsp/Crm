using ClosedXML.Excel;
using Crm.Entities.Contracts.Banking;
using Crm.Services.Common;

namespace Crm.Services.Banking
{
    public sealed class ExcelStatementExtractor : IStatementExtractor
    {
        public Task<ExtractResult> ExtractAsync(Stream file, string fileName, CancellationToken ct)
        {
            try
            {
                using var wb = new XLWorkbook(file);
                var ws = wb.Worksheets.First();

                // Başlık satırını bulma (MVP): ilk 25 satır
                var headerRow = -1;
                var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                for (int r = 1; r <= 25; r++)
                {
                    var used = ws.Row(r).CellsUsed().ToList();
                    if (used.Count < 4) continue;

                    var pairs = used
                        .Select(c => (col: c.Address.ColumnNumber, name: (c.GetString() ?? "").Trim()))
                        .Where(x => !string.IsNullOrWhiteSpace(x.name))
                        .ToList();

                    bool hasDate = pairs.Any(x => x.name.Equals("TARİH", StringComparison.OrdinalIgnoreCase));
                    bool hasDesc = pairs.Any(x => x.name.Equals("AÇIKLAMA", StringComparison.OrdinalIgnoreCase));
                    bool hasAmount = pairs.Any(x => x.name.Contains("TUTAR", StringComparison.OrdinalIgnoreCase));
                    bool hasBalance = pairs.Any(x => x.name.Contains("BAKİYE", StringComparison.OrdinalIgnoreCase));

                    if (hasDate && hasDesc && hasAmount && hasBalance)
                    {
                        headerRow = r;
                        foreach (var p in pairs)
                            headerMap[p.name] = p.col;
                        break;
                    }
                }

                if (headerRow < 0)
                    throw new ParseException("Excel başlık satırı bulunamadı. Beklenen kolonlar: TARİH/AÇIKLAMA/TUTAR/BAKİYE.");

                var last = ws.LastRowUsed().RowNumber();
                var rows = new List<RawRow>();

                for (int r = headerRow + 1; r <= last; r++)
                {
                    // Boş satırları atla
                    var dateCell = headerMap.TryGetValue("TARİH", out var dcol) ? ws.Cell(r, dcol).GetString() : null;
                    var descCell = headerMap.TryGetValue("AÇIKLAMA", out var ecol) ? ws.Cell(r, ecol).GetString() : null;
                    if (string.IsNullOrWhiteSpace(dateCell) && string.IsNullOrWhiteSpace(descCell))
                        continue;

                    var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                    foreach (var kv in headerMap)
                        dict[kv.Key] = ws.Cell(r, kv.Value).GetString();

                    rows.Add(new RawRow(r, dict));
                }

                return Task.FromResult(new ExtractResult(rows, ws.Name, $"Excel: {rows.Count} satır çıkarıldı"));
            }
            catch (ServiceException) { throw; }
            catch (Exception ex)
            {
                throw new ParseException($"Excel okuma hatası: {ex.Message}");
            }
        }
    }
}
