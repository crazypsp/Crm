namespace Crm.Api.Banking.Models.Responses
{
    public class ImportStatusResponse
    {
        public Guid ImportId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Progress { get; set; }
        public int TotalRows { get; set; }
        public int ProcessedRows { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}