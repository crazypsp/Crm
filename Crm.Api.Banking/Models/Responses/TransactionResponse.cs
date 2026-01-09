namespace Crm.Api.Banking.Models.Responses
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string MappingStatus { get; set; } = string.Empty;
        public string? SuggestedAccountCode { get; set; }
        public string? ApprovedAccountCode { get; set; }
    }
}