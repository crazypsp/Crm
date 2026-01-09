namespace Crm.Api.Admin.Models.Responses
{
    public class CompanyResponse
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TaxNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string TenantName { get; set; } = string.Empty;
    }
}