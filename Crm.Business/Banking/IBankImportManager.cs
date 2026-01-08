using Crm.Entities.Banking;

namespace Crm.Business.Banking
{
    public interface IBankImportManager
    {
        Task<BankStatementImport> CreateImportAsync(
            Guid tenantId,
            Guid companyId,
            Guid bankAccountId,
            Guid templateId,
            Guid sourceFileId,
            CancellationToken ct);

        Task AddTransactionsAsync(
            Guid tenantId,
            Guid importId,
            IReadOnlyList<BankTransaction> transactions,
            CancellationToken ct);

        Task ApplyMappingRulesAsync(
            Guid tenantId,
            Guid importId,
            CancellationToken ct);

        Task ApproveTransactionMappingAsync(
            Guid tenantId,
            Guid transactionId,
            string counterAccountCode,
            CancellationToken ct);

        Task<VoucherDraft> BuildVoucherDraftAsync(
            Guid tenantId,
            Guid importId,
            string bankAccountCode,
            CancellationToken ct);
    }
}
