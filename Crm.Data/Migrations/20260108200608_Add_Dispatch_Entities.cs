using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Crm.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Dispatch_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "AdapterKey",
                table: "IntegrationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "IntegrationProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "IntegrationProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SettingsJson",
                table: "IntegrationProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AutoFolderPath",
                table: "DocumentFiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FiscalMonth",
                table: "DocumentFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FiscalYear",
                table: "DocumentFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentDealerId",
                table: "Dealers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountingDatabaseName",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DatabaseCreatedByUserId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatabaseCreatedDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWelcomeEmailSent",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "WelcomeEmailSentByUserId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WelcomeEmailSentDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankTransactionTrainingData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DescriptionKeywordsJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsOutflow = table.Column<bool>(type: "bit", nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    CorrectAccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CorrectAccountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AppliedModelVersion = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModelConfidence = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsedInTraining = table.Column<bool>(type: "bit", nullable: false),
                    LastTrainingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactionTrainingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransactionTrainingData_BankTransactions_OriginalTransactionId",
                        column: x => x.OriginalTransactionId,
                        principalTable: "BankTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BulkFinancialDispatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    PeriodMonth = table.Column<int>(type: "int", nullable: true),
                    DispatchType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetCompaniesFilterJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TotalCompanies = table.Column<int>(type: "int", nullable: false),
                    SuccessfulDispatches = table.Column<int>(type: "int", nullable: false),
                    FailedDispatches = table.Column<int>(type: "int", nullable: false),
                    SummaryJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkFinancialDispatch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxCalendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    PeriodMonth = table.Column<int>(type: "int", nullable: false),
                    DeclarationDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeclarationSubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentCompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsAutoGenerated = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCalendar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxCalendar_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispatchItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BulkDispatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchItem_BulkFinancialDispatch_BulkDispatchId",
                        column: x => x.BulkDispatchId,
                        principalTable: "BulkFinancialDispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispatchItem_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { null, null, "Staff", "STAFF" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), null, null, "Admin", "ADMIN" },
                    { new Guid("44444444-4444-4444-4444-444444444442"), null, null, "Dealer", "DEALER" },
                    { new Guid("44444444-4444-4444-4444-444444444443"), null, null, "Accountant", "ACCOUNTANT" },
                    { new Guid("44444444-4444-4444-4444-444444444445"), null, null, "Company", "COMPANY" },
                    { new Guid("44444444-4444-4444-4444-444444444446"), null, null, "SubDealer", "SUBDEALER" }
                });

            migrationBuilder.InsertData(
                table: "BankMappingRules",
                columns: new[] { "Id", "CompanyId", "CounterAccountCode", "CounterAccountName", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "IsDeleted", "MatchType", "Name", "OnlyOutflow", "Pattern", "Priority", "ProgramType", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666680"), new Guid("33333333-3333-3333-3333-333333333333"), "361.01.001", "SGK Prim Borçları", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, 1, "SGK Ödemesi", true, "SGK", 10, null, new Guid("22222222-2222-2222-2222-222222222222"), null, null },
                    { new Guid("66666666-6666-6666-6666-666666666681"), new Guid("33333333-3333-3333-3333-333333333333"), "770.01.001", "Kira Giderleri", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, false, 1, "Kira", true, "KİRA", 20, null, new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                table: "BankTemplates",
                columns: new[] { "Id", "AmountNegativeMeansOutflow", "BankName", "ColumnMapJson", "CreatedAt", "CreatedByUserId", "CultureName", "DeletedAt", "DeletedByUserId", "IsActive", "IsDeleted", "Name", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666662"), true, "Demo Bank", "{\"date\":\"\\u0130\\u015Flem Tarihi\",\"desc\":\"A\\u00E7\\u0131klama\",\"amount\":\"Tutar\",\"balance\":\"Bakiye\",\"valueDate\":\"Val\\u00F6r Tarihi\"}", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "tr-TR", null, null, true, false, "Demo Ekstre Şablonu", new Guid("22222222-2222-2222-2222-222222222222"), null, null });

            migrationBuilder.InsertData(
                table: "BulkFinancialDispatch",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "DispatchType", "ExecutedDate", "FailedDispatches", "FiscalYear", "IsDeleted", "PeriodMonth", "ScheduledDate", "Status", "SuccessfulDispatches", "SummaryJson", "TargetCompaniesFilterJson", "TenantId", "Title", "TotalCompanies", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "TAX_CALENDAR", null, 0, 2026, false, null, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, null, "{\"IncludeAll\":true,\"ExcludeIds\":[]}", new Guid("22222222-2222-2222-2222-222222222222"), "2026 Yılı 1. Çeyrek Bildirimleri", 2, null, null });

            migrationBuilder.InsertData(
                table: "ConnectionSecrets",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "EncryptedJson", "IsDeleted", "Notes", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "{\"Password\":\"encrypted_password_here\",\"UserId\":\"sa\"}", false, "Demo connection secret", new Guid("22222222-2222-2222-2222-222222222222"), null, null });

            migrationBuilder.InsertData(
                table: "Dealers",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Email", "IsDeleted", "Name", "ParentDealerId", "Phone", "TaxNo", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "bayi@demo.local", false, "Demo Bayi", null, "05000000000", null, null, null });

            migrationBuilder.InsertData(
                table: "DocumentFiles",
                columns: new[] { "Id", "AutoFolderPath", "ContentType", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "FileName", "FiscalMonth", "FiscalYear", "IsDeleted", "Sha256", "SizeBytes", "StoragePath", "StorageProvider", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777771"), "2026/01/", "application/pdf", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "demo-belge.pdf", 1, 2026, false, null, 1024L, "tenant-22222222-2222-2222-2222-222222222222/company-33333333-3333-3333-3333-333333333333/2026/01/demo-belge.pdf", "local", new Guid("22222222-2222-2222-2222-222222222222"), null, null },
                    { new Guid("77777777-7777-7777-7777-777777777772"), "2026/01/", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "demo-ekstre.xlsx", 1, 2026, false, null, 2048L, "tenant-22222222-2222-2222-2222-222222222222/company-33333333-3333-3333-3333-333333333333/2026/01/demo-ekstre.xlsx", "local", new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                table: "Dealers",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Email", "IsDeleted", "Name", "ParentDealerId", "Phone", "TaxNo", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111112"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "altbayi@demo.local", false, "Demo Alt Bayi", new Guid("11111111-1111-1111-1111-111111111111"), "05000000001", null, null, null });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsActive", "IsDeleted", "OfficeName", "TaxNo", "TaxOffice", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "İstanbul/Kadıköy", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("11111111-1111-1111-1111-111111111111"), null, null, true, false, "Demo Mali Müşavir Ofisi", "11111111111", "Kadıköy", null, null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "DealerId", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555551"), 0, null, "78b1196f-d41f-4ca0-b40d-43579c54739e", new Guid("11111111-1111-1111-1111-111111111111"), "admin@demo.local", true, "Sistem Yöneticisi", true, false, null, "ADMIN@DEMO.LOCAL", "ADMIN@DEMO.LOCAL", "AQAAAAIAAYagAAAAEEhwbvVf5k28g3MsgJzIYwDf0/VQjWHkZn/nXJZm+Tux7BCWim5ZEj0YY30WQOlI/Q==", null, false, "18a08c74c6a348fdbd0f168f0a15d5d3", new Guid("22222222-2222-2222-2222-222222222222"), false, "admin@demo.local" },
                    { new Guid("55555555-5555-5555-5555-555555555553"), 0, null, "e0e99fa4-0628-45d3-8af2-4a05f37bf664", null, "musavir@demo.local", true, "Demo Mali Müşavir", true, false, null, "MUSAVIR@DEMO.LOCAL", "MUSAVIR@DEMO.LOCAL", "AQAAAAIAAYagAAAAEE2/UJNSf7LDahcKJNsnhSbkzEKqj/ALGvFd5P9LPBBgyril7R0JFSeQkU7z6SHHpA==", null, false, "bb49fb2de40444858660ddeac70e5405", new Guid("22222222-2222-2222-2222-222222222222"), false, "musavir@demo.local" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "AccountingDatabaseName", "Address", "CreatedAt", "CreatedByUserId", "DatabaseCreatedByUserId", "DatabaseCreatedDate", "DeletedAt", "DeletedByUserId", "Email", "IsActive", "IsDeleted", "IsWelcomeEmailSent", "Phone", "TaxNo", "TaxOffice", "TenantId", "Title", "UpdatedAt", "UpdatedByUserId", "WelcomeEmailSentByUserId", "WelcomeEmailSentDate" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "DemoFirmaA_DB", "İstanbul", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "firmaa@demo.local", true, false, true, "05000000000", "1234567890", "Kadıköy", new Guid("22222222-2222-2222-2222-222222222222"), "Demo Firma A", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("33333333-3333-3333-3333-333333333334"), "DemoFirmaB_DB", "İstanbul", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "firmab@demo.local", true, false, false, "05000000001", "9876543210", "Beşiktaş", new Guid("22222222-2222-2222-2222-222222222222"), "Demo Firma B", null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "DealerId", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), 0, new Guid("33333333-3333-3333-3333-333333333333"), "eb7085aa-c0a0-4508-aee7-f6093775d91a", null, "firma@demo.local", true, "Firma Yetkilisi", true, false, null, "FIRMA@DEMO.LOCAL", "FIRMA@DEMO.LOCAL", "AQAAAAIAAYagAAAAECPZRHf/BBv7w17N/a5U5BLdTtxSeXMdXleAZVxwgpsYEDBIUyiMtsQXTlItK/Hygg==", null, false, "ef04deadee6342b991365002f11646d9", null, false, "firma@demo.local" });

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountingBankAccountCode", "AccountingBankAccountName", "BankName", "CompanyId", "CreatedAt", "CreatedByUserId", "Currency", "DeletedAt", "DeletedByUserId", "Iban", "IsDeleted", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666661"), "102.01.001", "Demo Bank TL", "Demo Bank", new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "TRY", null, null, "TR000000000000000000000001", false, new Guid("22222222-2222-2222-2222-222222222222"), null, null });

            migrationBuilder.InsertData(
                table: "DispatchItem",
                columns: new[] { "Id", "BulkDispatchId", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "ErrorMessage", "IsDeleted", "IsSuccessful", "SentAt", "SentMethod", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"), new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, false, null, null, new Guid("22222222-2222-2222-2222-222222222222"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"), new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), new Guid("33333333-3333-3333-3333-333333333334"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, false, null, null, new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                table: "IntegrationProfiles",
                columns: new[] { "Id", "AdapterKey", "BranchCode", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "IsDefault", "IsDeleted", "Name", "ProgramType", "SecretId", "SettingsJson", "TenantId", "UpdatedAt", "UpdatedByUserId", "WorkplaceCode" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), "mssql", null, new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, true, true, false, "Demo Integration", 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), "{\"SqlServer\":\"localhost\\\\SQLEXPRESS\",\"SqlDatabase\":\"DemoFirmaA_DB\",\"SqlAuth\":\"Sql\"}", new Guid("22222222-2222-2222-2222-222222222222"), null, null, null });

            migrationBuilder.InsertData(
                table: "TaxCalendar",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DeclarationDueDate", "DeclarationSubmittedDate", "DeletedAt", "DeletedByUserId", "Description", "FiscalYear", "IsAutoGenerated", "IsCompleted", "IsDeleted", "Notes", "PaymentCompletedDate", "PaymentDueDate", "PeriodMonth", "TaxCode", "TenantId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "KDV 1. Taksit Beyannamesi", 2026, true, false, false, null, null, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KDV", new Guid("22222222-2222-2222-2222-222222222222"), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Ocak Ayı Muhtasar Beyannamesi", 2026, true, false, false, null, null, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MUHTASAR", new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[,]
                {
                    { new Guid("61212e3a-7af6-43b7-a41b-1acc2ae29ecd"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("11111111-1111-1111-1111-111111111111"), null, null, false, true, 1, null, null, null, new Guid("55555555-5555-5555-5555-555555555551") },
                    { new Guid("9d002bed-af85-49e5-9d3a-b96f1202e823"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 3, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("55555555-5555-5555-5555-555555555553") }
                });

            migrationBuilder.InsertData(
                table: "WorkTasks",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "DueAt", "IsDeleted", "Priority", "Status", "TenantId", "Title", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("88888888-8888-8888-8888-888888888881"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Demo seed görev açıklaması", new DateTimeOffset(new DateTime(2026, 1, 31, 17, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, 3, 1, new Guid("22222222-2222-2222-2222-222222222222"), "Demo görev: Ocak KDV kontrolü", null, null });

            migrationBuilder.InsertData(
                table: "BankStatementImports",
                columns: new[] { "Id", "BankAccountId", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "ImportedRows", "IsDeleted", "Notes", "SourceFileId", "Status", "TemplateId", "TenantId", "TotalRows", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666670"), new Guid("66666666-6666-6666-6666-666666666661"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, 3, false, "Demo import (seed)", new Guid("77777777-7777-7777-7777-777777777772"), 2, new Guid("66666666-6666-6666-6666-666666666662"), new Guid("22222222-2222-2222-2222-222222222222"), 3, null, null });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[] { new Guid("52caa0f3-e746-4f91-8be3-cfe7279768d3"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 5, null, null, null, new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.InsertData(
                table: "BankTransactions",
                columns: new[] { "Id", "Amount", "AppliedRuleId", "ApprovedCounterAccountCode", "BalanceAfter", "Confidence", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "ImportId", "IsDeleted", "MappingStatus", "ReferenceNo", "RowNo", "SuggestedCounterAccountCode", "TenantId", "TransactionDate", "UpdatedAt", "UpdatedByUserId", "ValueDate" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666671"), -15000m, new Guid("66666666-6666-6666-6666-666666666680"), null, 250000m, 0.90m, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "SGK PRİM ÖDEMESİ", new Guid("66666666-6666-6666-6666-666666666670"), false, 2, "REF001", 1, "361.01.001", new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("66666666-6666-6666-6666-666666666672"), -35000m, new Guid("66666666-6666-6666-6666-666666666681"), null, 215000m, 0.85m, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "OCAK KİRA ÖDEMESİ", new Guid("66666666-6666-6666-6666-666666666670"), false, 2, "REF002", 2, "770.01.001", new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("66666666-6666-6666-6666-666666666673"), 50000m, null, null, 265000m, null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "MÜŞTERİ TAHSİLATI", new Guid("66666666-6666-6666-6666-666666666670"), false, 1, "REF003", 3, null, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "VoucherDrafts",
                columns: new[] { "Id", "BankAccountCode", "CompanyId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "ImportId", "IsDeleted", "Status", "TenantId", "UpdatedAt", "UpdatedByUserId", "VoucherDate" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666690"), "102.01.001", new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Demo fiş taslağı (seed)", new Guid("66666666-6666-6666-6666-666666666670"), false, 1, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "BankTransactionTrainingData",
                columns: new[] { "Id", "Amount", "AppliedModelVersion", "CorrectAccountCode", "CorrectAccountName", "CreatedAt", "CreatedByUserId", "DayOfMonth", "DeletedAt", "DeletedByUserId", "DescriptionKeywordsJson", "IsDeleted", "IsOutflow", "LastTrainingDate", "ModelConfidence", "Month", "Notes", "OriginalTransactionId", "TenantId", "UpdatedAt", "UpdatedByUserId", "UsedInTraining" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-ddddddddddd1"), 15000m, null, "361.01.001", "SGK Prim Borçları", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 2, null, null, "[\"SGK\",\"PR\\u0130M\",\"\\u00D6DEME\"]", false, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.90m, 1, "Demo training data", new Guid("66666666-6666-6666-6666-666666666671"), new Guid("22222222-2222-2222-2222-222222222222"), null, null, true });

            migrationBuilder.InsertData(
                table: "VoucherDraftItems",
                columns: new[] { "Id", "BankTransactionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "FirstLineNo", "IsDeleted", "LastLineNo", "TenantId", "UpdatedAt", "UpdatedByUserId", "VoucherDraftId" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666693"), new Guid("66666666-6666-6666-6666-666666666671"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, 1, false, 2, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("66666666-6666-6666-6666-666666666690") });

            migrationBuilder.InsertData(
                table: "VoucherDraftLines",
                columns: new[] { "Id", "AccountCode", "AccountName", "CostCenterCode", "CreatedAt", "CreatedByUserId", "Credit", "Debit", "DeletedAt", "DeletedByUserId", "IsDeleted", "LineDescription", "LineNo", "TenantId", "UpdatedAt", "UpdatedByUserId", "VoucherDraftId" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666691"), "361.01.001", "SGK Prim Borçları", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 0m, 15000m, null, null, false, "SGK PRİM ÖDEMESİ", 1, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("66666666-6666-6666-6666-666666666690") },
                    { new Guid("66666666-6666-6666-6666-666666666692"), "102.01.001", "Demo Bank TL", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 15000m, 0m, null, null, false, "SGK PRİM ÖDEMESİ", 2, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("66666666-6666-6666-6666-666666666690") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationProfiles_TenantId_CompanyId_ProgramType_IsActive",
                table: "IntegrationProfiles",
                columns: new[] { "TenantId", "CompanyId", "ProgramType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_ParentDealerId",
                table: "Dealers",
                column: "ParentDealerId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMappingRules_TenantId_CompanyId_ProgramType_IsActive_Priority",
                table: "BankMappingRules",
                columns: new[] { "TenantId", "CompanyId", "ProgramType", "IsActive", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionTrainingData_OriginalTransactionId",
                table: "BankTransactionTrainingData",
                column: "OriginalTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItem_BulkDispatchId",
                table: "DispatchItem",
                column: "BulkDispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItem_CompanyId",
                table: "DispatchItem",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCalendar_CompanyId",
                table: "TaxCalendar",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dealers_Dealers_ParentDealerId",
                table: "Dealers",
                column: "ParentDealerId",
                principalTable: "Dealers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dealers_Dealers_ParentDealerId",
                table: "Dealers");

            migrationBuilder.DropTable(
                name: "BankTransactionTrainingData");

            migrationBuilder.DropTable(
                name: "DispatchItem");

            migrationBuilder.DropTable(
                name: "TaxCalendar");

            migrationBuilder.DropTable(
                name: "BulkFinancialDispatch");

            migrationBuilder.DropIndex(
                name: "IX_IntegrationProfiles_TenantId_CompanyId_ProgramType_IsActive",
                table: "IntegrationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Dealers_ParentDealerId",
                table: "Dealers");

            migrationBuilder.DropIndex(
                name: "IX_BankMappingRules_TenantId_CompanyId_ProgramType_IsActive_Priority",
                table: "BankMappingRules");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444445"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444446"));

            migrationBuilder.DeleteData(
                table: "BankMappingRules",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666680"));

            migrationBuilder.DeleteData(
                table: "BankMappingRules",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666681"));

            migrationBuilder.DeleteData(
                table: "BankTransactions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666672"));

            migrationBuilder.DeleteData(
                table: "BankTransactions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666673"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"));

            migrationBuilder.DeleteData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "DocumentFiles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777771"));

            migrationBuilder.DeleteData(
                table: "IntegrationProfiles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("52caa0f3-e746-4f91-8be3-cfe7279768d3"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("61212e3a-7af6-43b7-a41b-1acc2ae29ecd"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("9d002bed-af85-49e5-9d3a-b96f1202e823"));

            migrationBuilder.DeleteData(
                table: "VoucherDraftItems",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666693"));

            migrationBuilder.DeleteData(
                table: "VoucherDraftLines",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666691"));

            migrationBuilder.DeleteData(
                table: "VoucherDraftLines",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666692"));

            migrationBuilder.DeleteData(
                table: "WorkTasks",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888881"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555551"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555553"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "BankTransactions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666671"));

            migrationBuilder.DeleteData(
                table: "ConnectionSecrets",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"));

            migrationBuilder.DeleteData(
                table: "VoucherDrafts",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666690"));

            migrationBuilder.DeleteData(
                table: "BankStatementImports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666670"));

            migrationBuilder.DeleteData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666661"));

            migrationBuilder.DeleteData(
                table: "BankTemplates",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666662"));

            migrationBuilder.DeleteData(
                table: "DocumentFiles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777772"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DropColumn(
                name: "AdapterKey",
                table: "IntegrationProfiles");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "IntegrationProfiles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "IntegrationProfiles");

            migrationBuilder.DropColumn(
                name: "SettingsJson",
                table: "IntegrationProfiles");

            migrationBuilder.DropColumn(
                name: "AutoFolderPath",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "FiscalMonth",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "FiscalYear",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "ParentDealerId",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "AccountingDatabaseName",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DatabaseCreatedByUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DatabaseCreatedDate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsWelcomeEmailSent",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "WelcomeEmailSentByUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "WelcomeEmailSentDate",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { "role-staff", "Accountant staff", "MaliMusavirPersoneli", "MALIMUSAVIRPERSONELI" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "role-admin", "System administrator", "Admin", "ADMIN" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "role-dealer", "Dealer", "Bayi", "BAYI" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "role-accountant", "Accountant office owner", "MaliMusavir", "MALIMUSAVIR" },
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
    }
}
