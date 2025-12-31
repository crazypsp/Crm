namespace Crm.Data.Seeding;

public static class DemoSeedIds
{
    // Roles
    public static readonly Guid RoleAdmin = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid RoleDealer = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid RoleAccountant = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid RoleStaff = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid RoleCompany = Guid.Parse("55555555-5555-5555-5555-555555555555");

    // Users
    public static readonly Guid UserAdmin = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid UserDealer = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid UserAccountant = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    public static readonly Guid UserStaff = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
    public static readonly Guid UserCompany = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

    // Organization
    public static readonly Guid DealerDemo = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid TenantDemo = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid CompanyDemo = Guid.Parse("30000000-0000-0000-0000-000000000001");

    // Documents / Banking seed
    public static readonly Guid FileBankStatementDemo = Guid.Parse("90000000-0000-0000-0000-000000000001");
    public static readonly Guid BankAccountDemo = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid BankTemplateDemo = Guid.Parse("50000000-0000-0000-0000-000000000001");
    public static readonly Guid BankImportDemo = Guid.Parse("51000000-0000-0000-0000-000000000001");

    public static readonly Guid Tx1 = Guid.Parse("52000000-0000-0000-0000-000000000001");
    public static readonly Guid Tx2 = Guid.Parse("52000000-0000-0000-0000-000000000002");
    public static readonly Guid Tx3 = Guid.Parse("52000000-0000-0000-0000-000000000003");

    public static readonly Guid MappingRuleDemo = Guid.Parse("60000000-0000-0000-0000-000000000001");

    // Voucher draft seed
    public static readonly Guid VoucherDraftDemo = Guid.Parse("61000000-0000-0000-0000-000000000001");
    public static readonly Guid VLine1 = Guid.Parse("62000000-0000-0000-0000-000000000001");
    public static readonly Guid VLine2 = Guid.Parse("62000000-0000-0000-0000-000000000002");
    public static readonly Guid VLine3 = Guid.Parse("62000000-0000-0000-0000-000000000003");
    public static readonly Guid VItem1 = Guid.Parse("63000000-0000-0000-0000-000000000001");

    // Memberships
    public static readonly Guid MAdmin = Guid.Parse("70000000-0000-0000-0000-000000000001");
    public static readonly Guid MDealer = Guid.Parse("70000000-0000-0000-0000-000000000002");
    public static readonly Guid MAccountant = Guid.Parse("70000000-0000-0000-0000-000000000003");
    public static readonly Guid MStaff = Guid.Parse("70000000-0000-0000-0000-000000000004");
    public static readonly Guid MCompanyUser = Guid.Parse("70000000-0000-0000-0000-000000000005");
}
