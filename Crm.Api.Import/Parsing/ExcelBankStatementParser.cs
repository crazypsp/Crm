using Crm.Api.Import.Contracts;
using ExcelDataReader;
using System.Data;

namespace Crm.Api.Import.Parsing
{
    public sealed class ExcelBankStatementParser : IExcelBankStatementParser
    {
        public Task<PreviewBankStatementResponse> PreviewAsync(IFormFile file, CancellationToken ct)
        {
            using var stream = file.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var ds = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = false }
            });

            if (ds.Tables.Count == 0)
            {
                return Task.FromResult(new PreviewBankStatementResponse
                {
                    DetectedFormat = "excel",
                    Warnings = { "Excel içinde sheet bulunamadı." }
                });
            }

            var table = ds.Tables[0];

            // Neden: Banka dosyalarında header satırı her zaman 1. satır olmaz.
            var headerRow = FindHeaderRow(table, Math.Min(30, table.Rows.Count));
            if (headerRow < 0)
            {
                return Task.FromResult(new PreviewBankStatementResponse
                {
                    DetectedFormat = "excel",
                    Warnings = { "Header satırı tespit edilemedi (template/map gerekebilir)." }
                });
            }

            var header = GetRowStrings(table, headerRow);
            var map = DetectColumnMap(header);

            var warnings = new List<string>();
            ValidateMap(map, warnings);

            var rows = new List<BankStatementRowDto>();
            var rowNo = 0;

            for (int r = headerRow + 1; r < table.Rows.Count; r++)
            {
                var cells = GetRowStrings(table, r);
                if (cells.All(string.IsNullOrWhiteSpace)) continue;

                if (!TryReadDate(cells, map.DateCol, out var date)) continue;

                var desc = ReadString(cells, map.DescCol);
                var debit = ParseDecimalTr(ReadString(cells, map.DebitCol));
                var credit = ParseDecimalTr(ReadString(cells, map.CreditCol));
                var balance = ParseDecimalTr(ReadString(cells, map.BalanceCol));

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

            return Task.FromResult(new PreviewBankStatementResponse
            {
                DetectedFormat = "excel",
                Warnings = warnings.Distinct().Take(50).ToList(),
                Rows = rows
            });
        }

        private static int FindHeaderRow(DataTable table, int maxScanRows)
        {
            for (int r = 0; r < maxScanRows; r++)
            {
                var cells = GetRowStrings(table, r);
                var joined = string.Join(" ", cells).ToLowerInvariant();

                // Neden: TR bankalarında genelde “Tarih” + “Açıklama” birlikte gelir.
                if (joined.Contains("tarih") && (joined.Contains("açıklama") || joined.Contains("aciklama")))
                    return r;

                if (joined.Contains("işlem") && joined.Contains("tarih"))
                    return r;
            }
            return -1;
        }

        private static string[] GetRowStrings(DataTable table, int rowIndex)
        {
            var row = table.Rows[rowIndex];
            var arr = new string[table.Columns.Count];
            for (int c = 0; c < table.Columns.Count; c++)
                arr[c] = row[c]?.ToString()?.Trim() ?? "";
            return arr;
        }

        private static string ReadString(string[] cells, int col)
            => (col >= 0 && col < cells.Length) ? cells[col] : "";

        private static bool TryReadDate(string[] cells, int col, out DateTime date)
        {
            date = default;
            var s = ReadString(cells, col);
            if (DateTime.TryParse(s, out date)) return true;
            if (DateTime.TryParseExact(s, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date)) return true;
            return false;
        }

        private static decimal ParseDecimalTr(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            s = s.Trim();

            var negative = s.StartsWith("(") && s.EndsWith(")");
            s = s.Trim('(', ')');

            s = s.Replace("TL", "", StringComparison.OrdinalIgnoreCase).Replace("₺", "").Trim();

            // TR: 1.234,56 -> 1234.56
            var tr = s.Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(tr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var d))
                return negative ? -d : d;

            // fallback
            if (decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d))
                return negative ? -d : d;

            return 0m;
        }

        private sealed class ColMap
        {
            public int DateCol { get; set; } = -1;
            public int DescCol { get; set; } = -1;
            public int DebitCol { get; set; } = -1;
            public int CreditCol { get; set; } = -1;
            public int BalanceCol { get; set; } = -1;
        }

        private static ColMap DetectColumnMap(string[] header)
        {
            int Find(params string[] keys)
            {
                for (int i = 0; i < header.Length; i++)
                {
                    var h = (header[i] ?? "").ToLowerInvariant();
                    foreach (var k in keys)
                        if (h.Contains(k)) return i;
                }
                return -1;
            }

            return new ColMap
            {
                DateCol = Find("tarih", "işlem tarih", "islem tarih"),
                DescCol = Find("açıklama", "aciklama", "açiklama"),
                DebitCol = Find("borç", "borc", "çıkış", "cikis"),
                CreditCol = Find("alacak", "giriş", "giris"),
                BalanceCol = Find("bakiye", "son bakiye", "kalan")
            };
        }

        private static void ValidateMap(ColMap map, List<string> warnings)
        {
            if (map.DateCol < 0) warnings.Add("Tarih kolonu bulunamadı.");
            if (map.DescCol < 0) warnings.Add("Açıklama kolonu bulunamadı.");
            if (map.BalanceCol < 0) warnings.Add("Bakiye kolonu bulunamadı.");
            if (map.DebitCol < 0 && map.CreditCol < 0) warnings.Add("Borç/Alacak kolonları bulunamadı (tek tutar kolonu olabilir).");
        }
    }
}
