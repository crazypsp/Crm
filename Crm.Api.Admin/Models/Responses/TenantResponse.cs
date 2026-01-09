namespace Crm.Api.Admin.Models.Responses
{
    public class TenantResponse
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CompanyCount { get; set; }
        public int UserCount { get; set; }
    }
}