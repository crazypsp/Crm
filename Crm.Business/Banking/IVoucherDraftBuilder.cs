using Crm.Entities.Banking;

namespace Crm.Business.Banking
{
    public interface IVoucherDraftBuilder
    {
        VoucherDraft Build(
            Guid tenantId,
            Guid companyId,
            Guid importId,
            string bankAccountCode,
            IReadOnlyList<BankTransaction> transactions);
    }
}
