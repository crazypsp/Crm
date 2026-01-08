namespace Crm.Data.Seeding;

public static class DemoSeedIds
{
    // Tenancy
    public static readonly Guid Dealer1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid Dealer2 = Guid.Parse("11111111-1111-1111-1111-111111111112");
    public static readonly Guid Tenant1 = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid Company1 = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid Company2 = Guid.Parse("33333333-3333-3333-3333-333333333334");

    // Identity
    public static readonly Guid AdminRole = Guid.Parse("44444444-4444-4444-4444-444444444441");
    public static readonly Guid DealerRole = Guid.Parse("44444444-4444-4444-4444-444444444442");
    public static readonly Guid AccountantRole = Guid.Parse("44444444-4444-4444-4444-444444444443");
    public static readonly Guid StaffRole = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid CompanyRole = Guid.Parse("44444444-4444-4444-4444-444444444445");
    public static readonly Guid SubDealerRole = Guid.Parse("44444444-4444-4444-4444-444444444446");

    public static readonly Guid AdminUser = Guid.Parse("55555555-5555-5555-5555-555555555551");
    public static readonly Guid DealerUser = Guid.Parse("55555555-5555-5555-5555-555555555552");
    public static readonly Guid AccountantUser = Guid.Parse("55555555-5555-5555-5555-555555555553");
    public static readonly Guid StaffUser = Guid.Parse("55555555-5555-5555-5555-555555555554");
    public static readonly Guid CompanyUser = Guid.Parse("55555555-5555-5555-5555-555555555555");

    // Banking
    public static readonly Guid BankAccount1 = Guid.Parse("66666666-6666-6666-6666-666666666661");
    public static readonly Guid BankTemplate1 = Guid.Parse("66666666-6666-6666-6666-666666666662");

    public static readonly Guid BankStatementImport1 = Guid.Parse("66666666-6666-6666-6666-666666666670");
    public static readonly Guid BankTx1 = Guid.Parse("66666666-6666-6666-6666-666666666671");
    public static readonly Guid BankTx2 = Guid.Parse("66666666-6666-6666-6666-666666666672");
    public static readonly Guid BankTx3 = Guid.Parse("66666666-6666-6666-6666-666666666673");

    public static readonly Guid BankRule1 = Guid.Parse("66666666-6666-6666-6666-666666666680");
    public static readonly Guid BankRule2 = Guid.Parse("66666666-6666-6666-6666-666666666681");

    public static readonly Guid VoucherDraft1 = Guid.Parse("66666666-6666-6666-6666-666666666690");
    public static readonly Guid VoucherLine1 = Guid.Parse("66666666-6666-6666-6666-666666666691");
    public static readonly Guid VoucherLine2 = Guid.Parse("66666666-6666-6666-6666-666666666692");
    public static readonly Guid VoucherItem1 = Guid.Parse("66666666-6666-6666-6666-666666666693");

    // Documents
    public static readonly Guid DocumentFile1 = Guid.Parse("77777777-7777-7777-7777-777777777771");
    public static readonly Guid DocumentFile2 = Guid.Parse("77777777-7777-7777-7777-777777777772");

    // Work
    public static readonly Guid Task1 = Guid.Parse("88888888-8888-8888-8888-888888888881");

    // Messaging
    public static readonly Guid Thread1 = Guid.Parse("99999999-9999-9999-9999-999999999991");
    public static readonly Guid Message1 = Guid.Parse("99999999-9999-9999-9999-999999999992");

    // Integration
    public static readonly Guid IntegrationProfile1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
    public static readonly Guid ConnectionSecret1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
    public static readonly Guid ChartOfAccount1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");

    // Tax (Yeni eklenenler)
    public static readonly Guid TaxCalendar1 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1");
    public static readonly Guid TaxCalendar2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2");

    // Dispatch (Yeni eklenenler)
    public static readonly Guid BulkDispatch1 = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1");
    public static readonly Guid DispatchItem1 = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc2");
    public static readonly Guid DispatchItem2 = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3");

    // AI Training (Yeni eklenenler)
    public static readonly Guid TrainingData1 = Guid.Parse("dddddddd-dddd-dddd-dddd-ddddddddddd1");
}