using Crm.Entities.Tenancy;

namespace Crm.Business.Tenancy
{
    public interface ITenancyManager
    {
        Task<Tenant> CreateTenantAsync(Guid dealerId, string officeName, CancellationToken ct);
        Task<Company> CreateCompanyAsync(Guid tenantId, string title, string? taxNo, CancellationToken ct);
        Task<Tenant> GetTenantAsync(Guid tenantId, CancellationToken ct);
        Task<Company> GetCompanyAsync(Guid tenantId, Guid companyId, CancellationToken ct);
    }
}
