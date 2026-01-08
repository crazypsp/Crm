using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Tenancy
{
    public sealed class TenancyManager : ITenancyManager
    {
        private readonly CrmDbContext _db;

        public TenancyManager(CrmDbContext db)
        {
            _db = db;
        }

        public async Task<Tenant> CreateTenantAsync(Guid dealerId, string officeName, CancellationToken ct)
        {
            Guard.NotEmpty(dealerId, nameof(dealerId));
            Guard.NotBlank(officeName, nameof(officeName));

            var dealerExists = await _db.Dealers
                .AnyAsync(x => x.Id == dealerId && !x.IsDeleted, ct);
            if (!dealerExists)
                throw new NotFoundException("Bayi bulunamadı.");

            var tenant = new Tenant
            {
                DealerId = dealerId,
                OfficeName = officeName.Trim(),
                IsActive = true
            };

            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync(ct);

            return tenant;
        }

        public async Task<Company> CreateCompanyAsync(Guid tenantId, string title, string? taxNo, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotBlank(title, nameof(title));

            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(x => x.Id == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Tenant (mali müşavir ofisi) bulunamadı.");

            if (!tenant.IsActive)
                throw new ValidationException("Tenant pasif durumda.");

            var company = new Company
            {
                TenantId = tenantId,
                Title = title.Trim(),
                TaxNo = string.IsNullOrWhiteSpace(taxNo) ? null : taxNo.Trim(),
                IsActive = true
            };

            _db.Companies.Add(company);
            await _db.SaveChangesAsync(ct);

            return company;
        }

        public async Task<Tenant> GetTenantAsync(Guid tenantId, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));

            var tenant = await _db.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Tenant bulunamadı.");

            return tenant;
        }

        public async Task<Company> GetCompanyAsync(Guid tenantId, Guid companyId, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(companyId, nameof(companyId));

            var company = await _db.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == companyId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Firma bulunamadı veya yetkisiz erişim.");

            return company;
        }
    }
}
