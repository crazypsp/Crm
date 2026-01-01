using Crm.Entities.Banking;
using Crm.Entities.Enums;
using System.Text.RegularExpressions;

namespace Crm.Business.Banking
{
    public sealed class BankMappingEngine : IBankMappingEngine
    {
        public void ApplyRules(IReadOnlyList<BankMappingRule> rules, IReadOnlyList<BankTransaction> txs)
        {
            foreach (var tx in txs)
            {
                if (tx.MappingStatus == MappingStatus.Approved) continue;

                foreach (var rule in rules)
                {
                    if (!rule.IsActive) continue;

                    if (rule.OnlyOutflow is true && tx.Amount >= 0) continue;
                    if (rule.OnlyOutflow is false && tx.Amount <= 0) continue;

                    var ok = rule.MatchType switch
                    {
                        Entities.Enums.MatchType.Contains => tx.Description.Contains(rule.Pattern, StringComparison.OrdinalIgnoreCase),
                        Entities.Enums.MatchType.StartsWith => tx.Description.StartsWith(rule.Pattern, StringComparison.OrdinalIgnoreCase),
                        Entities.Enums.MatchType.Equals => string.Equals(tx.Description.Trim(), rule.Pattern.Trim(), StringComparison.OrdinalIgnoreCase),
                        Entities.Enums.MatchType.Regex => Regex.IsMatch(tx.Description, rule.Pattern, RegexOptions.IgnoreCase),
                        _ => false
                    };

                    if (!ok) continue;

                    tx.SuggestedCounterAccountCode = rule.CounterAccountCode;
                    tx.AppliedRuleId = rule.Id;
                    tx.MappingStatus = MappingStatus.Suggested;
                    tx.Confidence = 0.80m; // MVP sabit; sonra dinamikleştirilebilir
                    break;
                }
            }
        }
    }
}
