using Crm.Entities.Banking;
using Crm.Entities.Contracts.Banking;

namespace Crm.Services.Banking
{
    public interface IBankStatementNormalizer
    {
        IReadOnlyList<BankTransaction> NormalizeExcelRows(
            Guid tenantId,
            Guid importId,
            IReadOnlyDictionary<string, string> columnMap,
            IReadOnlyList<RawRow> rows);
    }
}