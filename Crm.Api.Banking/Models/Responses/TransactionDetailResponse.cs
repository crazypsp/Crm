namespace Crm.Api.Banking.Models.Responses
{
    public class TransactionDetailResponse
    {
        public Guid Id { get; set; }
        public Guid ImportId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string? ReferenceNo { get; set; }
        public int RowNo { get; set; }
        public string MappingStatus { get; set; } = string.Empty;
        public string? SuggestedAccountCode { get; set; }
        public string? SuggestedAccountName { get; set; }
        public decimal? Confidence { get; set; }
        public string? ApprovedAccountCode { get; set; }
        public Guid? AppliedRuleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}