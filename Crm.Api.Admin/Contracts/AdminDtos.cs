namespace Crm.Api.Admin.Contracts
{
    public sealed class CreateDealerRequest
    {
        public string Name { get; set; } = default!;
        public string? Notes { get; set; }
    }

    public sealed class DealerDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public sealed class CreateTenantRequest
    {
        public Guid DealerId { get; set; }
        public string Name { get; set; } = default!;
    }

    public sealed class TenantDto
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public string? Name { get; set; }
    }

    public sealed class CreateCompanyRequest
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string? TaxNo { get; set; }
    }

    public sealed class CompanyDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string? Name { get; set; }
        public string? TaxNo { get; set; }
    }

    public sealed class CreateUserRequest
    {
        // Neden: Identity tarafı.
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;

        // Neden: Kullanıcı tipine göre Role atayacağız.
        // Örn: "Admin", "Dealer", "Accountant", "Staff", "Company"
        public string Role { get; set; } = default!;

        // Neden: Tenant/Company bağlamı membership ile yönetilecek.
        public Guid? TenantId { get; set; }
        public Guid? CompanyId { get; set; }
    }

    public sealed class UserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public sealed class UpsertMembershipRequest
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }

        // Örn: "Dealer", "Accountant", "Staff", "Company"
        public string MembershipRole { get; set; } = default!;
    }
}
