using Crm.Api.Import.Contracts;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;

namespace Crm.Api.Import.Parsing
{
    public sealed class PdfBankStatementParser : IPdfBankStatementParser
    {
        private static readonly Regex DateRx = new(@"(?<d>\d{2}\.\d{2}\.\d{4})", RegexOptions.Compiled);
        private static readonly Regex MoneyRx = new(@"-?\(?\d{1,3}(\.\d{3})*(,\d{2})\)?", RegexOptions.Compiled);

        public Task<PreviewBankStatementResponse> PreviewAsync(IFormFile file, CancellationToken ct)
        {
            using var stream = file.OpenReadStream();
            using var doc = PdfDocument.Open(stream);

            var warnings = new List<string>();
            var rows = new List<BankStatementRowDto>();
            var rowNo = 0;

            foreach (var page in doc.GetPages())
            {
                var text = page.Text ?? "";
                var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var line in lines)
                {
                    var dm = DateRx.Match(line);
                    if (!dm.Success) continue;

                    if (!DateTime.TryParseExact(dm.Groups["d"].Value, "dd.MM.yyyy", null,
                            System.Globalization.DateTimeStyles.None, out var date))
                        continue;

                    var monies = MoneyRx.Matches(line).Select(m => m.Value).ToList();
                    if (monies.Count == 0) continue;

                    var balance = ParseDecimalTr(monies.Last());

                    decimal debit = 0m, credit = 0m;

                    // Neden: PDF satırları bozulabilir; basit heuristics ile borç/alacak ayrımı yaparız.
                    if (monies.Count >= 3)
                    {
                        debit = ParseDecimalTr(monies[^3]);
                        credit = ParseDecimalTr(monies[^2]);
                    }
                    else if (monies.Count == 2)
                    {
                        var amount = ParseDecimalTr(monies[0]);
                        var lower = line.ToLowerInvariant();
                        if (lower.Contains("borç") || lower.Contains("borc") || amount < 0)
                            debit = Math.Abs(amount);
                        else
                            credit = Math.Abs(amount);
                    }
                    else
                    {
                        warnings.Add("PDF satırlarında borç/alacak net tespit edilemedi (template gerekebilir).");
                    }

                    var desc = line.Replace(dm.Groups["d"].Value, "").Trim();
                    foreach (var m in monies) desc = desc.Replace(m, "").Trim();
                    desc = Regex.Replace(desc, @"\s{2,}", " ").Trim();

                    rowNo++;
                    rows.Add(new BankStatementRowDto
                    {
                        RowNo = rowNo,
                        TransactionDate = date,
                        Description = string.IsNullOrWhiteSpace(desc) ? "(Açıklama yok)" : desc,
                        Debit = debit,
                        Credit = credit,
                        Balance = balance
                    });
                }
            }

            if (rows.Count == 0)
                warnings.Add("PDF metin içermiyor olabilir (görüntü PDF). OCR gerekebilir.");

            return Task.FromResult(new PreviewBankStatementResponse
            {
                DetectedFormat = "pdf",
                Warnings = warnings.Distinct().Take(50).ToList(),
                Rows = rows
            });
        }

        private static decimal ParseDecimalTr(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            s = s.Trim();

            var negative = s.StartsWith("(") && s.EndsWith(")");
            s = s.Trim('(', ')');

            var tr = s.Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(tr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var d))
                return negative ? -d : d;

            return 0m;
        }
    }
}
