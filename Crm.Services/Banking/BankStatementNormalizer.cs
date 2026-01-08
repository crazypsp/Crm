using Crm.Entities.Banking;
using Crm.Entities.Contracts.Banking;
using Crm.Services.Common;
using System.Globalization;

namespace Crm.Services.Banking
{
    public sealed class BankStatementNormalizer : IBankStatementNormalizer
    {
        private static readonly CultureInfo Tr = CultureInfo.GetCultureInfo("tr-TR");

        public IReadOnlyList<BankTransaction> NormalizeExcelRows(
            Guid tenantId,
            Guid importId,
            IReadOnlyDictionary<string, string> map,
            IReadOnlyList<RawRow> rows)
        {
            DateTime ParseDate(string? s)
            {
                if (string.IsNullOrWhiteSpace(s))
                    throw new ParseException("Tarih alanı boş.");

                return DateTime.ParseExact(s.Trim(), "dd.MM.yyyy", Tr);
            }

            decimal ParseMoney(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return 0m;
                var x = s.Trim().Replace(".", "").Replace(",", ".");
                return decimal.Parse(x, CultureInfo.InvariantCulture);
            }

            string? Get(RawRow rr, string excelHeader)
                => rr.Cells.TryGetValue(excelHeader, out var v) ? v : null;

            var list = new List<BankTransaction>();

            foreach (var rr in rows)
            {
                if (!map.TryGetValue("date", out var dateHeader) ||
                    !map.TryGetValue("desc", out var descHeader) ||
                    !map.TryGetValue("amount", out var amountHeader) ||
                    !map.TryGetValue("balance", out var balanceHeader))
                    throw new ParseException("ColumnMapJson zorunlu anahtarları içermiyor: date/desc/amount/balance");

                var tx = new BankTransaction
                {
                    TenantId = tenantId,
                    ImportId = importId,
                    RowNo = rr.RowNo,
                    TransactionDate = ParseDate(Get(rr, dateHeader)),
                    Description = (Get(rr, descHeader) ?? "").Trim(),
                    Amount = ParseMoney(Get(rr, amountHeader)),
                    BalanceAfter = ParseMoney(Get(rr, balanceHeader))
                };

                if (map.TryGetValue("valueDate", out var vdHeader))
                {
                    var vd = Get(rr, vdHeader);
                    tx.ValueDate = string.IsNullOrWhiteSpace(vd) ? null : ParseDate(vd);
                }

                if (map.TryGetValue("ref", out var refHeader))
                    tx.ReferenceNo = Get(rr, refHeader)?.Trim();

                list.Add(tx);
            }

            return list;
        }
    }
}