using Crm.Business.Common;
using Crm.Entities.Banking;

namespace Crm.Business.Banking
{
    public sealed class VoucherDraftBuilder : IVoucherDraftBuilder
    {
        public VoucherDraft Build(
            Guid tenantId,
            Guid companyId,
            Guid importId,
            string bankAccountCode,
            IReadOnlyList<BankTransaction> transactions)
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
                VoucherDate = transactions.Count == 0 ? DateTime.Today : transactions.Min(x => x.TransactionDate),
                Description = "Banka Ekstresi Fişi",
                BankAccountCode = bankAccountCode.Trim()
            };

            var lineNo = 1;

            foreach (var transaction in transactions)
            {
                var counter = transaction.ApprovedCounterAccountCode ?? transaction.SuggestedCounterAccountCode;
                if (string.IsNullOrWhiteSpace(counter))
                    continue;

                var amount = Math.Abs(transaction.Amount);

                if (transaction.Amount < 0) // Çıkış
                {
                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = counter,
                        Debit = amount,
                        Credit = 0m,
                        LineDescription = transaction.Description
                    });

                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = bankAccountCode,
                        Debit = 0m,
                        Credit = amount,
                        LineDescription = transaction.Description
                    });
                }
                else // Giriş
                {
                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = bankAccountCode,
                        Debit = amount,
                        Credit = 0m,
                        LineDescription = transaction.Description
                    });

                    draft.Lines.Add(new VoucherDraftLine
                    {
                        TenantId = tenantId,
                        LineNo = lineNo++,
                        AccountCode = counter,
                        Debit = 0m,
                        Credit = amount,
                        LineDescription = transaction.Description
                    });
                }
            }

            var debit = draft.Lines.Sum(x => x.Debit);
            var credit = draft.Lines.Sum(x => x.Credit);

            if (debit != credit)
                throw new ValidationException($"Fiş dengesiz: Borç={debit}, Alacak={credit}");

            return draft;
        }
    }
}
