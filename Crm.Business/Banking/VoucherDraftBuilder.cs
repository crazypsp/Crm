using Crm.Business.Common;
using Crm.Entities.Banking;

namespace Crm.Business.Banking
{
    public sealed class VoucherDraftBuilder : IVoucherDraftBuilder
    {
        public VoucherDraft Build(Guid tenantId, Guid companyId, Guid importId, string bankAccountCode, IReadOnlyList<BankTransaction> txs)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(companyId, nameof(companyId));
            Guard.NotEmpty(importId, nameof(importId));
            Guard.NotBlank(bankAccountCode, nameof(bankAccountCode));

            var draft = new VoucherDraft
            {
                TenantId = tenantId,
                CompanyId = companyId,
                ImportId = importId,
                VoucherDate = txs.Count == 0 ? DateTime.Today : txs.Min(x => x.TransactionDate),
                Description = "Banka Ekstresi Fişi",
                BankAccountCode = bankAccountCode.Trim()
            };

            var lineNo = 1;

            foreach (var tx in txs)
            {
                var counter = tx.ApprovedCounterAccountCode ?? tx.SuggestedCounterAccountCode;
                if (string.IsNullOrWhiteSpace(counter))
                    continue; // MVP: eşleşmeyen satır kullanıcıya bırakılır

                var amt = Math.Abs(tx.Amount);

                // Çıkış (Amount < 0): Gider BORÇ, Banka ALACAK
                if (tx.Amount < 0)
                {
                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = counter,
                        Debit = amt,
                        Credit = 0m,
                        LineDescription = tx.Description
                    });
                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = bankAccountCode,
                        Debit = 0m,
                        Credit = amt,
                        LineDescription = tx.Description
                    });
                }
                // Giriş (Amount > 0): Banka BORÇ, Gelir/karşı hesap ALACAK
                else
                {
                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = bankAccountCode,
                        Debit = amt,
                        Credit = 0m,
                        LineDescription = tx.Description
                    });
                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = counter,
                        Debit = 0m,
                        Credit = amt,
                        LineDescription = tx.Description
                    });
                }
            }

            // Fiş bütünlüğü (debit==credit) iş kuralıdır
            var debit = draft.Lines.Sum(x => x.Debit);
            var credit = draft.Lines.Sum(x => x.Credit);
            if (debit != credit)
                throw new ValidationException($"Fiş dengesiz: Borç={debit}, Alacak={credit}");

            return draft;
        }
    }
}
