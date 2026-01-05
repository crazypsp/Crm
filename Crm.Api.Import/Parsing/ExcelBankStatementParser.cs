using Crm.Api.Import.Contracts;
using ExcelDataReader;
using System.Data;
using System.Globalization;

namespace Crm.Api.Import.Parsing
{
    public sealed class ExcelBankStatementParser : IBankStatementParser
    {
        public StatementFileType FileType => StatementFileType.Excel;

        public async Task<IReadOnlyList<PreviewBankStatementRow>> ParseAsync(Stream fileStream, CancellationToken ct)
        {
            // ExcelDataReader sync çalışır; wrapper olarak Task.Run kullanıyoruz.
            // Neden: Web request thread’ini bloklamamak.
            return await Task.Run(() =>
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using var reader = ExcelReaderFactory.CreateReader(fileStream);
                var rows = new List<PreviewBankStatementRow>();

                var rowNo = 0;
                while (reader.Read())
                {
                    ct.ThrowIfCancellationRequested();

                    rowNo++;

                    // MVP: ilk satır header ise atlayabilirsin (istersen Template ile yönet)
                    // Burada basit örnek: tarih kolonunu parse edebiliyorsak satır kabul ediyoruz.
                    var dateRaw = reader.GetValue(0)?.ToString();
                    if (!TryParseDate(dateRaw, out var txDate))
                        continue;

                    var desc = reader.GetValue(1)?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(desc))
                        desc = "(Açıklama yok)";

                    var debitRaw = reader.GetValue(2)?.ToString();
                    var creditRaw = reader.GetValue(3)?.ToString();
                    var balanceRaw = reader.GetValue(4)?.ToString();

                    var debit = TryParseMoney(debitRaw);
                    var credit = TryParseMoney(creditRaw);
                    var balance = TryParseMoney(balanceRaw);

                    MoneyDirection dir;
                    decimal amount;

                    if (credit > 0)
                    {
                        dir = MoneyDirection.Credit;
                        amount = credit;
                    }
                    else
                    {
                        dir = MoneyDirection.Debit;
                        amount = debit;
                    }

                    if (amount <= 0) continue;

                    rows.Add(new PreviewBankStatementRow
                    {
                        RowNo = rowNo,
                        TxDate = txDate,
                        Description = desc.Trim(),
                        Amount = amount,
                        Direction = dir,
                        BalanceAfter = balance
                    });
                }

                return (IReadOnlyList<PreviewBankStatementRow>)rows;
            }, ct);
        }

        private static bool TryParseDate(string? raw, out DateOnly date)
        {
            date = default;

            if (string.IsNullOrWhiteSpace(raw)) return false;

            // Excel bazı durumlarda OADate döner.
            if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var oa))
            {
                // OADate aralığına benziyorsa
                if (oa > 30000 && oa < 60000)
                {
                    var dt = DateTime.FromOADate(oa);
                    date = DateOnly.FromDateTime(dt);
                    return true;
                }
            }

            // TR formatlar
            if (DateTime.TryParse(raw, new CultureInfo("tr-TR"), DateTimeStyles.None, out var d1))
            {
                date = DateOnly.FromDateTime(d1);
                return true;
            }

            // ISO formatlar
            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d2))
            {
                date = DateOnly.FromDateTime(d2);
                return true;
            }

            return false;
        }

        private static decimal TryParseMoney(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return 0;

            // "1.234,56" ve "1234.56" gibi formatları normalize et
            raw = raw.Trim();

            // TR -> invariant normalize
            // 1.234,56 -> 1234.56
            raw = raw.Replace(".", "").Replace(",", ".");

            if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var m))
                return m;

            return 0;
        }
    }
}
