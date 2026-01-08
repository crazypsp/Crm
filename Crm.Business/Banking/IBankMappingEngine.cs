using Crm.Entities.Banking;

namespace Crm.Business.Banking
{
    public interface IBankMappingEngine
    {
        void ApplyRules(
            IReadOnlyList<BankMappingRule> rules,
            IReadOnlyList<BankTransaction> transactions);
    }
}
