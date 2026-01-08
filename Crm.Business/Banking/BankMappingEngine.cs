using Crm.Entities.Banking;
using Crm.Entities.Enums;
using System.Text.RegularExpressions;
using EntitiesMatchType = Crm.Entities.Enums.MatchType; // Alias tanımlama

namespace Crm.Business.Banking
{
    public sealed class BankMappingEngine : IBankMappingEngine
    {
        public void ApplyRules(
            IReadOnlyList<BankMappingRule> rules,
            IReadOnlyList<BankTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.MappingStatus == MappingStatus.Approved)
                    continue;

                foreach (var rule in rules)
                {
                    if (!rule.IsActive)
                        continue;

                    if (rule.OnlyOutflow is true && transaction.Amount >= 0)
                        continue;
                    if (rule.OnlyOutflow is false && transaction.Amount <= 0)
                        continue;

                    var isMatch = rule.MatchType switch
                    {
                        // Alias kullanarak
                        EntitiesMatchType.Contains => transaction.Description.Contains(
                            rule.Pattern, StringComparison.OrdinalIgnoreCase),
                        EntitiesMatchType.StartsWith => transaction.Description.StartsWith(
                            rule.Pattern, StringComparison.OrdinalIgnoreCase),
                        EntitiesMatchType.Equals => string.Equals(
                            transaction.Description.Trim(),
                            rule.Pattern.Trim(),
                            StringComparison.OrdinalIgnoreCase),
                        EntitiesMatchType.Regex => Regex.IsMatch(
                            transaction.Description,
                            rule.Pattern,
                            RegexOptions.IgnoreCase),
                        _ => false
                    };

                    if (!isMatch)
                        continue;

                    transaction.SuggestedCounterAccountCode = rule.CounterAccountCode;
                    transaction.AppliedRuleId = rule.Id;
                    transaction.MappingStatus = MappingStatus.Suggested;
                    transaction.Confidence = 0.80m;
                    break;
                }
            }
        }
    }
}
