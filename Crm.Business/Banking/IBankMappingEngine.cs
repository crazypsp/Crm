using Crm.Entities.Banking;

namespace Crm.Business.Banking
{
    public interface IBankMappingEngine
    {
        /// <summary>
        /// Transaction listesine kural bazlı öneri uygular.
        /// DB yazma BankImportManager tarafından yapılır; engine saf (pure) kalsın diye.
        /// </summary>
        void ApplyRules(IReadOnlyList<BankMappingRule> rules, IReadOnlyList<BankTransaction> transactions);
    }
}
