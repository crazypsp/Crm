using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Crm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "role-admin", "System administrator", "Admin", "ADMIN" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "role-dealer", "Dealer", "Bayi", "BAYI" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "role-accountant", "Accountant office owner", "MaliMusavir", "MALIMUSAVIR" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "role-staff", "Accountant staff", "MaliMusavirPersoneli", "MALIMUSAVIRPERSONELI" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "role-company", "Company user", "Firma", "FIRMA" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "DealerId", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 0, null, "con-admin", null, "admin@demo.local", true, "Demo Admin", true, false, null, "ADMIN@DEMO.LOCAL", "ADMIN@DEMO.LOCAL", "<PASTE_PASSWORD_HASH_HERE>", null, false, "sec-admin", null, false, "admin@demo.local" });

            migrationBuilder.InsertData(
                table: "BankMappingRules",
                columns: new[] { "Id", "CompanyId", "CounterAccountCode", "CounterAccountName", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "IsDeleted", "MatchType", "Name", "OnlyOutflow", "Pattern", "Priority", "ProgramType", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("60000000-0000-0000-0000-000000000001"), null, "770.01.001", "Genel Yönetim Giderleri", new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, 1, "POS Harcamaları", true, "POS", 10, null, new Guid("20000000-0000-0000-0000-000000000001"), null, null });

            migrationBuilder.InsertData(
                table: "BankTemplates",
                columns: new[] { "Id", "AmountNegativeMeansOutflow", "BankName", "ColumnMapJson", "CreatedAt", "CreatedByUserId", "CultureName", "DeletedAt", "DeletedByUserId", "IsActive", "IsDeleted", "Name", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), true, "Demo Bank", "{\r\n  \"date\": \"TARİH\",\r\n  \"valueDate\": \"VALÖR\",\r\n  \"ref\": \"REF NO\",\r\n  \"amount\": \"İŞLEM TUTARI\",\r\n  \"balance\": \"BAKİYE\",\r\n  \"desc\": \"AÇIKLAMA\"\r\n}", new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "tr-TR", null, null, true, false, "Demo Ekstre Şablonu (Excel)", new Guid("20000000-0000-0000-0000-000000000001"), null, null });

            migrationBuilder.InsertData(
                table: "Dealers",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Email", "IsDeleted", "Name", "Phone", "TaxNo", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "dealer@demo.local", false, "Demo Bayi", "0000", "1111111111", null, null });

            migrationBuilder.InsertData(
                table: "DocumentFiles",
                columns: new[] { "Id", "ContentType", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "FileName", "IsDeleted", "Sha256", "SizeBytes", "StoragePath", "StorageProvider", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("90000000-0000-0000-0000-000000000001"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "demo-bank-statement.xlsx", false, null, 12345L, "seed/demo-bank-statement.xlsx", "local", new Guid("20000000-0000-0000-0000-000000000001"), null, null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "DealerId", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, "con-dealer", new Guid("10000000-0000-0000-0000-000000000001"), "dealer@demo.local", true, "Demo Bayi", true, false, null, "DEALER@DEMO.LOCAL", "DEALER@DEMO.LOCAL", "<PASTE_PASSWORD_HASH_HERE>", null, false, "sec-dealer", null, false, "dealer@demo.local" });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsActive", "IsDeleted", "OfficeName", "TaxNo", "TaxOffice", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), "Demo Address", new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("10000000-0000-0000-0000-000000000001"), null, null, true, false, "Demo Mali Müşavir Ofisi", "2222222222", "Demo Vergi Dairesi", null, null });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[] { new Guid("70000000-0000-0000-0000-000000000001"), null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 1, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "DealerId", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 0, null, "con-acc", new Guid("10000000-0000-0000-0000-000000000001"), "accountant@demo.local", true, "Demo Mali Müşavir", true, false, null, "ACCOUNTANT@DEMO.LOCAL", "ACCOUNTANT@DEMO.LOCAL", "<PASTE_PASSWORD_HASH_HERE>", null, false, "sec-acc", new Guid("20000000-0000-0000-0000-000000000001"), false, "accountant@demo.local" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 0, null, "con-staff", null, "staff@demo.local", true, "Demo Personel", true, false, null, "STAFF@DEMO.LOCAL", "STAFF@DEMO.LOCAL", "<PASTE_PASSWORD_HASH_HERE>", null, false, "sec-staff", new Guid("20000000-0000-0000-0000-000000000001"), false, "staff@demo.local" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Email", "IsActive", "IsDeleted", "Phone", "TaxNo", "TaxOffice", "TenantId", "Title", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("30000000-0000-0000-0000-000000000001"), "Demo Company Address", new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "company@demo.local", true, false, "0000", "3333333333", "Demo Vergi Dairesi", new Guid("20000000-0000-0000-0000-000000000001"), "Demo Mükellef A.Ş.", null, null });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[] { new Guid("70000000-0000-0000-0000-000000000002"), null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("10000000-0000-0000-0000-000000000001"), null, null, false, true, 2, null, null, null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "DealerId", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), 0, new Guid("30000000-0000-0000-0000-000000000001"), "con-company", null, "company@demo.local", true, "Demo Firma Kullanıcısı", true, false, null, "COMPANY@DEMO.LOCAL", "COMPANY@DEMO.LOCAL", "<PASTE_PASSWORD_HASH_HERE>", null, false, "sec-company", new Guid("20000000-0000-0000-0000-000000000001"), false, "company@demo.local" });

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountingBankAccountCode", "AccountingBankAccountName", "BankName", "CompanyId", "CreatedAt", "CreatedByUserId", "Currency", "DeletedAt", "DeletedByUserId", "Iban", "IsDeleted", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("40000000-0000-0000-0000-000000000001"), "102.01.001", "Demo Banka Hesabı", "Demo Bank", new Guid("30000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "TRY", null, null, "TR000000000000000000000000", false, new Guid("20000000-0000-0000-0000-000000000001"), null, null });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000003"), null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("10000000-0000-0000-0000-000000000001"), null, null, false, true, 3, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 4, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.InsertData(
                table: "BankStatementImports",
                columns: new[] { "Id", "BankAccountId", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "ImportedRows", "IsDeleted", "Notes", "SourceFileId", "Status", "TemplateId", "TenantId", "TotalRows", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("51000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, 3, false, "Demo import", new Guid("90000000-0000-0000-0000-000000000001"), 4, new Guid("50000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), 3, null, null });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[] { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 5, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.InsertData(
                table: "BankTransactions",
                columns: new[] { "Id", "Amount", "AppliedRuleId", "ApprovedCounterAccountCode", "BalanceAfter", "Confidence", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "ImportId", "IsDeleted", "MappingStatus", "ReferenceNo", "RowNo", "SuggestedCounterAccountCode", "TenantId", "TransactionDate", "UpdatedAt", "UpdatedByUserId", "ValueDate" },
                values: new object[,]
                {
                    { new Guid("52000000-0000-0000-0000-000000000001"), -600000.00m, new Guid("60000000-0000-0000-0000-000000000001"), "770.01.001", 1400000.00m, 0.95m, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "POS HARCAMA MARKET", new Guid("51000000-0000-0000-0000-000000000001"), false, 3, "REF001", 2, "770.01.001", new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("52000000-0000-0000-0000-000000000002"), 250000.00m, null, null, 1650000.00m, null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "HAVALE GELİŞİ", new Guid("51000000-0000-0000-0000-000000000001"), false, 1, "REF002", 3, null, new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("52000000-0000-0000-0000-000000000003"), -50000.00m, new Guid("60000000-0000-0000-0000-000000000001"), null, 1600000.00m, 0.80m, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "POS HARCAMA AKARYAKIT", new Guid("51000000-0000-0000-0000-000000000001"), false, 2, "REF003", 4, "770.01.001", new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "VoucherDrafts",
                columns: new[] { "Id", "BankAccountCode", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "ImportId", "IsDeleted", "Status", "TenantId", "UpdatedAt", "UpdatedByUserId", "VoucherDate" },
                values: new object[] { new Guid("61000000-0000-0000-0000-000000000001"), "102.01.001", new Guid("30000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Demo Banka Fişi", new Guid("51000000-0000-0000-0000-000000000001"), false, 1, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "VoucherDraftItems",
                columns: new[] { "Id", "BankTransactionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "FirstLineNo", "IsDeleted", "LastLineNo", "TenantId", "UpdatedAt", "UpdatedByUserId", "VoucherDraftId" },
                values: new object[] { new Guid("63000000-0000-0000-0000-000000000001"), new Guid("52000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, 1, false, 2, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new Guid("61000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "VoucherDraftLines",
                columns: new[] { "Id", "AccountCode", "AccountName", "CostCenterCode", "CreatedAt", "CreatedByUserId", "Credit", "Debit", "DeletedAt", "DeletedByUserId", "IsDeleted", "LineDescription", "LineNo", "TenantId", "UpdatedAt", "UpdatedByUserId", "VoucherDraftId" },
                values: new object[,]
                {
                    { new Guid("62000000-0000-0000-0000-000000000001"), "770.01.001", "Genel Yönetim Giderleri", null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 0m, 600000.00m, null, null, false, "POS HARCAMA MARKET", 1, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new Guid("61000000-0000-0000-0000-000000000001") },
                    { new Guid("62000000-0000-0000-0000-000000000002"), "102.01.001", "Demo Banka Hesabı", null, new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 600000.00m, 0m, null, null, false, "POS HARCAMA MARKET", 2, new Guid("20000000-0000-0000-0000-000000000001"), null, null, new Guid("61000000-0000-0000-0000-000000000001") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                table: "BankMappingRules",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "BankTransactions",
                keyColumn: "Id",
                keyValue: new Guid("52000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "BankTransactions",
                keyColumn: "Id",
                keyValue: new Guid("52000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "VoucherDraftItems",
                keyColumn: "Id",
                keyValue: new Guid("63000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "VoucherDraftLines",
                keyColumn: "Id",
                keyValue: new Guid("62000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "VoucherDraftLines",
                keyColumn: "Id",
                keyValue: new Guid("62000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "BankTransactions",
                keyColumn: "Id",
                keyValue: new Guid("52000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "VoucherDrafts",
                keyColumn: "Id",
                keyValue: new Guid("61000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "BankStatementImports",
                keyColumn: "Id",
                keyValue: new Guid("51000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "BankTemplates",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "DocumentFiles",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));
        }
    }
}
