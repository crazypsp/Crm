using System.Text.Json;
using Crm.Entities.Banking;
using Crm.Entities.Banking.AI;
using Crm.Entities.Dispatch;
using Crm.Entities.Documents;
using Crm.Entities.Enums;
using Crm.Entities.Identity;
using Crm.Entities.Integration;
using Crm.Entities.Tax;
using Crm.Entities.Tenancy;
using Crm.Entities.Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crm.Data.Seeding;

public static class DemoSeedData
{
    public static void Apply(ModelBuilder b)
    {
        // Tüm seed verilerinde kullanılacak SABIT tarih
        var fixedDate = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);

        // ==========================
        // 1) Tenancy: Dealer/Tenant/Company
        // ==========================
        b.Entity<Dealer>().HasData(
            new Dealer
            {
                Id = DemoSeedIds.Dealer1,
                Name = "Demo Bayi",
                Email = "bayi@demo.local",
                Phone = "05000000000",
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new Dealer
            {
                Id = DemoSeedIds.Dealer2,
                Name = "Demo Alt Bayi",
                ParentDealerId = DemoSeedIds.Dealer1,
                Email = "altbayi@demo.local",
                Phone = "05000000001",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<Tenant>().HasData(
            new Tenant
            {
                Id = DemoSeedIds.Tenant1,
                DealerId = DemoSeedIds.Dealer1,
                OfficeName = "Demo Mali Müşavir Ofisi",
                TaxOffice = "Kadıköy",
                TaxNo = "11111111111",
                Address = "İstanbul/Kadıköy",
                IsActive = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<Company>().HasData(
            new Company
            {
                Id = DemoSeedIds.Company1,
                TenantId = DemoSeedIds.Tenant1,
                Title = "Demo Firma A",
                TaxOffice = "Kadıköy",
                TaxNo = "1234567890",
                Email = "firmaa@demo.local",
                Phone = "05000000000",
                Address = "İstanbul",
                IsActive = true,
                AccountingDatabaseName = "DemoFirmaA_DB",
                DatabaseCreatedDate = fixedDate.DateTime,
                IsWelcomeEmailSent = true,
                WelcomeEmailSentDate = fixedDate.DateTime,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new Company
            {
                Id = DemoSeedIds.Company2,
                TenantId = DemoSeedIds.Tenant1,
                Title = "Demo Firma B",
                TaxOffice = "Beşiktaş",
                TaxNo = "9876543210",
                Email = "firmab@demo.local",
                Phone = "05000000001",
                Address = "İstanbul",
                IsActive = true,
                AccountingDatabaseName = "DemoFirmaB_DB",
                DatabaseCreatedDate = fixedDate.DateTime,
                IsWelcomeEmailSent = false,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 2) Identity: Role + User
        // ==========================
        b.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = DemoSeedIds.AdminRole,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "admin-concurrency-stamp" // Sabit değer
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.DealerRole,
                Name = "Dealer",
                NormalizedName = "DEALER",
                ConcurrencyStamp = "dealer-concurrency-stamp"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.SubDealerRole,
                Name = "SubDealer",
                NormalizedName = "SUBDEALER",
                ConcurrencyStamp = "subdealer-concurrency-stamp"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.AccountantRole,
                Name = "Accountant",
                NormalizedName = "ACCOUNTANT",
                ConcurrencyStamp = "accountant-concurrency-stamp"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.StaffRole,
                Name = "Staff",
                NormalizedName = "STAFF",
                ConcurrencyStamp = "staff-concurrency-stamp"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.CompanyRole,
                Name = "Company",
                NormalizedName = "COMPANY",
                ConcurrencyStamp = "company-concurrency-stamp"
            }
        );

        // Sabit Security Stamp'ler (Guid.NewGuid() yerine)
        string adminSecurityStamp = "11111111-1111-1111-1111-111111111111";
        string accountantSecurityStamp = "22222222-2222-2222-2222-222222222222";
        string companySecurityStamp = "33333333-3333-3333-3333-333333333333";

        // Admin User
        b.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = DemoSeedIds.AdminUser,
                UserName = "admin@demo.local",
                NormalizedUserName = "ADMIN@DEMO.LOCAL",
                Email = "admin@demo.local",
                NormalizedEmail = "ADMIN@DEMO.LOCAL",
                EmailConfirmed = true,
                SecurityStamp = adminSecurityStamp,
                ConcurrencyStamp = "admin-user-concurrency-stamp",
                PasswordHash = "AQAAAAEAACcQAAAAEHRDILxLq7nLd5AL3KJQcHlL4jNQ2hTp9hK5mR8vJ1kKjHqYtZwXvB2nMjPpLk3Gzg==", // "P@ssw0rd!" hash'i
                FullName = "Sistem Yöneticisi",
                IsActive = true,
                DealerId = DemoSeedIds.Dealer1,
                TenantId = DemoSeedIds.Tenant1
            },
            new ApplicationUser
            {
                Id = DemoSeedIds.AccountantUser,
                UserName = "musavir@demo.local",
                NormalizedUserName = "MUSAVIR@DEMO.LOCAL",
                Email = "musavir@demo.local",
                NormalizedEmail = "MUSAVIR@DEMO.LOCAL",
                EmailConfirmed = true,
                SecurityStamp = accountantSecurityStamp,
                ConcurrencyStamp = "accountant-user-concurrency-stamp",
                PasswordHash = "AQAAAAEAACcQAAAAEHRDILxLq7nLd5AL3KJQcHlL4jNQ2hTp9hK5mR8vJ1kKjHqYtZwXvB2nMjPpLk3Gzg==",
                FullName = "Demo Mali Müşavir",
                IsActive = true,
                TenantId = DemoSeedIds.Tenant1
            },
            new ApplicationUser
            {
                Id = DemoSeedIds.CompanyUser,
                UserName = "firma@demo.local",
                NormalizedUserName = "FIRMA@DEMO.LOCAL",
                Email = "firma@demo.local",
                NormalizedEmail = "FIRMA@DEMO.LOCAL",
                EmailConfirmed = true,
                SecurityStamp = companySecurityStamp,
                ConcurrencyStamp = "company-user-concurrency-stamp",
                PasswordHash = "AQAAAAEAACcQAAAAEHRDILxLq7nLd5AL3KJQcHlL4jNQ2hTp9hK5mR8vJ1kKjHqYtZwXvB2nMjPpLk3Gzg==",
                FullName = "Firma Yetkilisi",
                IsActive = true,
                CompanyId = DemoSeedIds.Company1
            }
        );

        // UserMembership (Roller)
        b.Entity<UserMembership>().HasData(
            new UserMembership
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                UserId = DemoSeedIds.AdminUser,
                Role = MembershipRole.Admin,
                DealerId = DemoSeedIds.Dealer1,
                IsPrimary = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new UserMembership
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                UserId = DemoSeedIds.AccountantUser,
                Role = MembershipRole.Accountant,
                TenantId = DemoSeedIds.Tenant1,
                IsPrimary = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new UserMembership
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                UserId = DemoSeedIds.CompanyUser,
                Role = MembershipRole.CompanyUser,
                CompanyId = DemoSeedIds.Company1,
                IsPrimary = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 3) Documents: Demo Dosyalar
        // ==========================
        b.Entity<DocumentFile>().HasData(
            new DocumentFile
            {
                Id = DemoSeedIds.DocumentFile1,
                TenantId = DemoSeedIds.Tenant1,
                FileName = "demo-belge.pdf",
                ContentType = "application/pdf",
                SizeBytes = 1024,
                StorageProvider = "local",
                StoragePath = $"tenant-{DemoSeedIds.Tenant1}/company-{DemoSeedIds.Company1}/2026/01/demo-belge.pdf",
                FiscalYear = 2026,
                FiscalMonth = 1,
                AutoFolderPath = "2026/01/",
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new DocumentFile
            {
                Id = DemoSeedIds.DocumentFile2,
                TenantId = DemoSeedIds.Tenant1,
                FileName = "demo-ekstre.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                SizeBytes = 2048,
                StorageProvider = "local",
                StoragePath = $"tenant-{DemoSeedIds.Tenant1}/company-{DemoSeedIds.Company1}/2026/01/demo-ekstre.xlsx",
                FiscalYear = 2026,
                FiscalMonth = 1,
                AutoFolderPath = "2026/01/",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 4) Banking: BankAccount + Template
        // ==========================
        var columnMap = new
        {
            date = "İşlem Tarihi",
            desc = "Açıklama",
            amount = "Tutar",
            balance = "Bakiye",
            valueDate = "Valör Tarihi"
        };

        b.Entity<BankAccount>().HasData(
            new BankAccount
            {
                Id = DemoSeedIds.BankAccount1,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                BankName = "Demo Bank",
                Iban = "TR000000000000000000000001",
                Currency = "TRY",
                AccountingBankAccountCode = "102.01.001",
                AccountingBankAccountName = "Demo Bank TL",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<BankTemplate>().HasData(
            new BankTemplate
            {
                Id = DemoSeedIds.BankTemplate1,
                TenantId = DemoSeedIds.Tenant1,
                Name = "Demo Ekstre Şablonu",
                BankName = "Demo Bank",
                ColumnMapJson = JsonSerializer.Serialize(columnMap),
                CultureName = "tr-TR",
                AmountNegativeMeansOutflow = true,
                IsActive = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 5) Banking: Mapping Rules
        // ==========================
        b.Entity<BankMappingRule>().HasData(
            new BankMappingRule
            {
                Id = DemoSeedIds.BankRule1,
                TenantId = DemoSeedIds.Tenant1,
                Name = "SGK Ödemesi",
                MatchType = Entities.Enums.MatchType.Contains,
                Pattern = "SGK",
                ProgramType = null,
                CompanyId = DemoSeedIds.Company1,
                CounterAccountCode = "361.01.001",
                CounterAccountName = "SGK Prim Borçları",
                OnlyOutflow = true,
                Priority = 10,
                IsActive = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new BankMappingRule
            {
                Id = DemoSeedIds.BankRule2,
                TenantId = DemoSeedIds.Tenant1,
                Name = "Kira",
                MatchType = Entities.Enums.MatchType.Contains,
                Pattern = "KİRA",
                ProgramType = null,
                CompanyId = DemoSeedIds.Company1,
                CounterAccountCode = "770.01.001",
                CounterAccountName = "Kira Giderleri",
                OnlyOutflow = true,
                Priority = 20,
                IsActive = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 6) Import demo: BankStatementImport + Transactions
        // ==========================
        b.Entity<BankStatementImport>().HasData(
            new BankStatementImport
            {
                Id = DemoSeedIds.BankStatementImport1,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                BankAccountId = DemoSeedIds.BankAccount1,
                TemplateId = DemoSeedIds.BankTemplate1,
                SourceFileId = DemoSeedIds.DocumentFile2,
                Status = BankImportStatus.Extracted,
                TotalRows = 3,
                ImportedRows = 3,
                Notes = "Demo import (seed)",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<BankTransaction>().HasData(
            new BankTransaction
            {
                Id = DemoSeedIds.BankTx1,
                TenantId = DemoSeedIds.Tenant1,
                ImportId = DemoSeedIds.BankStatementImport1,
                TransactionDate = new DateTime(2026, 01, 02),
                ValueDate = new DateTime(2026, 01, 02),
                ReferenceNo = "REF001",
                Description = "SGK PRİM ÖDEMESİ",
                Amount = -15000m,
                BalanceAfter = 250000m,
                RowNo = 1,
                MappingStatus = MappingStatus.Suggested,
                SuggestedCounterAccountCode = "361.01.001",
                ApprovedCounterAccountCode = null,
                AppliedRuleId = DemoSeedIds.BankRule1,
                Confidence = 0.90m,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new BankTransaction
            {
                Id = DemoSeedIds.BankTx2,
                TenantId = DemoSeedIds.Tenant1,
                ImportId = DemoSeedIds.BankStatementImport1,
                TransactionDate = new DateTime(2026, 01, 03),
                ValueDate = new DateTime(2026, 01, 03),
                ReferenceNo = "REF002",
                Description = "OCAK KİRA ÖDEMESİ",
                Amount = -35000m,
                BalanceAfter = 215000m,
                RowNo = 2,
                MappingStatus = MappingStatus.Suggested,
                SuggestedCounterAccountCode = "770.01.001",
                ApprovedCounterAccountCode = null,
                AppliedRuleId = DemoSeedIds.BankRule2,
                Confidence = 0.85m,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new BankTransaction
            {
                Id = DemoSeedIds.BankTx3,
                TenantId = DemoSeedIds.Tenant1,
                ImportId = DemoSeedIds.BankStatementImport1,
                TransactionDate = new DateTime(2026, 01, 04),
                ValueDate = new DateTime(2026, 01, 04),
                ReferenceNo = "REF003",
                Description = "MÜŞTERİ TAHSİLATI",
                Amount = 50000m,
                BalanceAfter = 265000m,
                RowNo = 3,
                MappingStatus = MappingStatus.Unmapped,
                SuggestedCounterAccountCode = null,
                ApprovedCounterAccountCode = null,
                AppliedRuleId = null,
                Confidence = null,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 7) Demo VoucherDraft
        // ==========================
        b.Entity<VoucherDraft>().HasData(
            new VoucherDraft
            {
                Id = DemoSeedIds.VoucherDraft1,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                ImportId = DemoSeedIds.BankStatementImport1,
                VoucherDate = new DateTime(2026, 01, 02),
                Description = "Demo fiş taslağı (seed)",
                BankAccountCode = "102.01.001",
                Status = VoucherDraftStatus.Draft,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<VoucherDraftLine>().HasData(
            new VoucherDraftLine
            {
                Id = DemoSeedIds.VoucherLine1,
                TenantId = DemoSeedIds.Tenant1,
                VoucherDraftId = DemoSeedIds.VoucherDraft1,
                LineNo = 1,
                AccountCode = "361.01.001",
                AccountName = "SGK Prim Borçları",
                Debit = 15000m,
                Credit = 0m,
                LineDescription = "SGK PRİM ÖDEMESİ",
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new VoucherDraftLine
            {
                Id = DemoSeedIds.VoucherLine2,
                TenantId = DemoSeedIds.Tenant1,
                VoucherDraftId = DemoSeedIds.VoucherDraft1,
                LineNo = 2,
                AccountCode = "102.01.001",
                AccountName = "Demo Bank TL",
                Debit = 0m,
                Credit = 15000m,
                LineDescription = "SGK PRİM ÖDEMESİ",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<VoucherDraftItem>().HasData(
            new VoucherDraftItem
            {
                Id = DemoSeedIds.VoucherItem1,
                TenantId = DemoSeedIds.Tenant1,
                VoucherDraftId = DemoSeedIds.VoucherDraft1,
                BankTransactionId = DemoSeedIds.BankTx1,
                FirstLineNo = 1,
                LastLineNo = 2,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 8) Work Tasks
        // ==========================
        b.Entity<WorkTask>().HasData(
            new WorkTask
            {
                Id = DemoSeedIds.Task1,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                Title = "Demo görev: Ocak KDV kontrolü",
                Description = "Demo seed görev açıklaması",
                Status = WorkTaskStatus.Open,
                Priority = 3,
                DueAt = new DateTimeOffset(2026, 01, 31, 17, 0, 0, TimeSpan.Zero),
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 9) Integration Demo
        // ==========================
        b.Entity<ConnectionSecret>().HasData(
            new ConnectionSecret
            {
                Id = DemoSeedIds.ConnectionSecret1,
                TenantId = DemoSeedIds.Tenant1,
                EncryptedJson = JsonSerializer.Serialize(new
                {
                    Password = "encrypted_password_here",
                    UserId = "sa"
                }),
                Notes = "Demo connection secret",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<IntegrationProfile>().HasData(
            new IntegrationProfile
            {
                Id = DemoSeedIds.IntegrationProfile1,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                Name = "Demo Integration",
                AdapterKey = "mssql",
                SettingsJson = JsonSerializer.Serialize(new
                {
                    SqlServer = "localhost\\SQLEXPRESS",
                    SqlDatabase = "DemoFirmaA_DB",
                    SqlAuth = "Sql"
                }),
                ProgramType = ProgramType.Zirve,
                SecretId = DemoSeedIds.ConnectionSecret1,
                IsDefault = true,
                IsActive = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 10) TAX: Mali Takvim Demo
        // ==========================
        b.Entity<TaxCalendar>().HasData(
            new TaxCalendar
            {
                Id = DemoSeedIds.TaxCalendar1,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                TaxCode = "KDV",
                Description = "KDV 1. Taksit Beyannamesi",
                FiscalYear = 2026,
                PeriodMonth = 1,
                DeclarationDueDate = new DateTime(2026, 02, 26),
                PaymentDueDate = new DateTime(2026, 02, 26),
                IsCompleted = false,
                IsAutoGenerated = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new TaxCalendar
            {
                Id = DemoSeedIds.TaxCalendar2,
                TenantId = DemoSeedIds.Tenant1,
                CompanyId = DemoSeedIds.Company1,
                TaxCode = "MUHTASAR",
                Description = "Ocak Ayı Muhtasar Beyannamesi",
                FiscalYear = 2026,
                PeriodMonth = 1,
                DeclarationDueDate = new DateTime(2026, 02, 23),
                PaymentDueDate = new DateTime(2026, 02, 26),
                IsCompleted = false,
                IsAutoGenerated = true,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 11) DISPATCH: Toplu Gönderim Demo
        // ==========================
        b.Entity<BulkFinancialDispatch>().HasData(
            new BulkFinancialDispatch
            {
                Id = DemoSeedIds.BulkDispatch1,
                TenantId = DemoSeedIds.Tenant1,
                Title = "2026 Yılı 1. Çeyrek Bildirimleri",
                FiscalYear = 2026,
                PeriodMonth = null,
                DispatchType = "TAX_CALENDAR",
                ScheduledDate = new DateTime(2026, 01, 15),
                ExecutedDate = null,
                TargetCompaniesFilterJson = JsonSerializer.Serialize(new
                {
                    IncludeAll = true,
                    ExcludeIds = new List<Guid>()
                }),
                TotalCompanies = 2,
                SuccessfulDispatches = 0,
                FailedDispatches = 0,
                Status = DispatchStatus.Scheduled,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        b.Entity<DispatchItem>().HasData(
            new DispatchItem
            {
                Id = DemoSeedIds.DispatchItem1,
                TenantId = DemoSeedIds.Tenant1,
                BulkDispatchId = DemoSeedIds.BulkDispatch1,
                CompanyId = DemoSeedIds.Company1,
                SentAt = null,
                SentMethod = null,
                ErrorMessage = null,
                IsSuccessful = false,
                CreatedAt = fixedDate,
                IsDeleted = false
            },
            new DispatchItem
            {
                Id = DemoSeedIds.DispatchItem2,
                TenantId = DemoSeedIds.Tenant1,
                BulkDispatchId = DemoSeedIds.BulkDispatch1,
                CompanyId = DemoSeedIds.Company2,
                SentAt = null,
                SentMethod = null,
                ErrorMessage = null,
                IsSuccessful = false,
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );

        // ==========================
        // 12) AI Training Data Demo
        // ==========================
        b.Entity<BankTransactionTrainingData>().HasData(
            new BankTransactionTrainingData
            {
                Id = DemoSeedIds.TrainingData1,
                TenantId = DemoSeedIds.Tenant1,
                OriginalTransactionId = DemoSeedIds.BankTx1,
                DescriptionKeywordsJson = JsonSerializer.Serialize(new[] { "SGK", "PRİM", "ÖDEME" }),
                Amount = 15000m,
                IsOutflow = true,
                DayOfMonth = 2,
                Month = 1,
                CorrectAccountCode = "361.01.001",
                CorrectAccountName = "SGK Prim Borçları",
                AppliedModelVersion = null,
                ModelConfidence = 0.90m,
                UsedInTraining = true,
                LastTrainingDate = fixedDate.DateTime,
                Notes = "Demo training data",
                CreatedAt = fixedDate,
                IsDeleted = false
            }
        );
    }
}