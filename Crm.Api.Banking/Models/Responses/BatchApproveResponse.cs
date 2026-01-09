namespace Crm.Api.Banking.Models.Responses
{
    public class BatchApproveResponse
    {
        public int TotalCount { get; set; }
        public int ApprovedCount { get; set; }
        public int FailedCount { get; set; }
        public List<Guid> FailedTransactionIds { get; set; } = new();
    }
}