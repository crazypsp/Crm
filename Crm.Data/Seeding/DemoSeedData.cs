using Crm.Entities.Banking;
using Crm.Entities.Documents;
using Crm.Entities.Enums;
using Crm.Entities.Identity;
using Crm.Entities.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crm.Data.Seeding;

public static class DemoSeedData
{
    // Seed zamanı sabit olmalı (deterministik migration)
    private static readonly DateTimeOffset SeedTime =
        new DateTimeOffset(2025, 12, 29, 0, 0, 0, TimeSpan.Zero);

    public static void Apply(ModelBuilder b)
    {
        SeedRoles(b);
        SeedUsersAndUserRoles(b);
        SeedOrganization(b);
        SeedBankingAndImport(b);
        SeedVoucherDraft(b);
        SeedMemberships(b);
    }

    private static void SeedRoles(ModelBuilder b)
    {
        // Identity rollerini seed ediyoruz: UI yetkilendirmesi bunlarla çalışacak.
        b.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = DemoSeedIds.RoleAdmin,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "System administrator",
                ConcurrencyStamp = "role-admin"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.RoleDealer,
                Name = "Bayi",
                NormalizedName = "BAYI",
                Description = "Dealer",
                ConcurrencyStamp = "role-dealer"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.RoleAccountant,
                Name = "MaliMusavir",
                NormalizedName = "MALIMUSAVIR",
                Description = "Accountant office owner",
                ConcurrencyStamp = "role-accountant"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.RoleStaff,
                Name = "MaliMusavirPersoneli",
                NormalizedName = "MALIMUSAVIRPERSONELI",
                Description = "Accountant staff",
                ConcurrencyStamp = "role-staff"
            },
            new ApplicationRole
            {
                Id = DemoSeedIds.RoleCompany,
                Name = "Firma",
                NormalizedName = "FIRMA",
                Description = "Company user",
                ConcurrencyStamp = "role-company"
            }
        );
    }

    private static void SeedUsersAndUserRoles(ModelBuilder b)
    {
        // ÖNEMLİ: PasswordHash sabit olmalı.
        // Aşağıdaki hash'i bir kere üretip buraya yapıştır:
        // Password: Demo123*
        const string DemoPasswordHash = "<PASTE_PASSWORD_HASH_HERE>";

        b.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = DemoSeedIds.UserAdmin,
                UserName = "admin@demo.local",
                NormalizedUserName = "ADMIN@DEMO.LOCAL",
                Email = "admin@demo.local",
                NormalizedEmail = "ADMIN@DEMO.LOCAL",
                EmailConfirmed = true,
                FullName = "Demo Admin",
                IsActive = true,
                SecurityStamp = "sec-admin",
                ConcurrencyStamp = "con-admin",
                PasswordHash = DemoPasswordHash
            },
            new ApplicationUser
            {
                Id = DemoSeedIds.UserDealer,
                UserName = "dealer@demo.local",
                NormalizedUserName = "DEALER@DEMO.LOCAL",
                Email = "dealer@demo.local",
                NormalizedEmail = "DEALER@DEMO.LOCAL",
                EmailConfirmed = true,
                FullName = "Demo Bayi",
                IsActive = true,
                SecurityStamp = "sec-dealer",
                ConcurrencyStamp = "con-dealer",
                PasswordHash = DemoPasswordHash,
                DealerId = DemoSeedIds.DealerDemo
            },
            new ApplicationUser
            {
                Id = DemoSeedIds.UserAccountant,
                UserName = "accountant@demo.local",
                NormalizedUserName = "ACCOUNTANT@DEMO.LOCAL",
                Email = "accountant@demo.local",
                NormalizedEmail = "ACCOUNTANT@DEMO.LOCAL",
                EmailConfirmed = true,
                FullName = "Demo Mali Müşavir",
                IsActive = true,
                SecurityStamp = "sec-acc",
                ConcurrencyStamp = "con-acc",
                PasswordHash = DemoPasswordHash,
                DealerId = DemoSeedIds.DealerDemo,
                TenantId = DemoSeedIds.TenantDemo
            },
            new ApplicationUser
            {
                Id = DemoSeedIds.UserStaff,
                UserName = "staff@demo.local",
                NormalizedUserName = "STAFF@DEMO.LOCAL",
                Email = "staff@demo.local",
                NormalizedEmail = "STAFF@DEMO.LOCAL",
                EmailConfirmed = true,
                FullName = "Demo Personel",
                IsActive = true,
                SecurityStamp = "sec-staff",
                ConcurrencyStamp = "con-staff",
                PasswordHash = DemoPasswordHash,
                TenantId = DemoSeedIds.TenantDemo
            },
            new ApplicationUser
            {
                Id = DemoSeedIds.UserCompany,
                UserName = "company@demo.local",
                NormalizedUserName = "COMPANY@DEMO.LOCAL",
                Email = "company@demo.local",
                NormalizedEmail = "COMPANY@DEMO.LOCAL",
                EmailConfirmed = true,
                FullName = "Demo Firma Kullanıcısı",
                IsActive = true,
                SecurityStamp = "sec-company",
                ConcurrencyStamp = "con-company",
                PasswordHash = DemoPasswordHash,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = DemoSeedIds.CompanyDemo
            }
        );

        // AspNetUserRoles tablosuna seed
        b.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = DemoSeedIds.UserAdmin, RoleId = DemoSeedIds.RoleAdmin },
            new IdentityUserRole<Guid> { UserId = DemoSeedIds.UserDealer, RoleId = DemoSeedIds.RoleDealer },
            new IdentityUserRole<Guid> { UserId = DemoSeedIds.UserAccountant, RoleId = DemoSeedIds.RoleAccountant },
            new IdentityUserRole<Guid> { UserId = DemoSeedIds.UserStaff, RoleId = DemoSeedIds.RoleStaff },
            new IdentityUserRole<Guid> { UserId = DemoSeedIds.UserCompany, RoleId = DemoSeedIds.RoleCompany }
        );
    }

    private static void SeedOrganization(ModelBuilder b)
    {
        // Bayi → Tenant → Company hiyerarşisini demo olarak kuruyoruz.
        b.Entity<Dealer>().HasData(
            new Dealer
            {
                Id = DemoSeedIds.DealerDemo,
                Name = "Demo Bayi",
                Email = "dealer@demo.local",
                Phone = "0000",
                TaxNo = "1111111111",
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        b.Entity<Tenant>().HasData(
            new Tenant
            {
                Id = DemoSeedIds.TenantDemo,
                DealerId = DemoSeedIds.DealerDemo,
                OfficeName = "Demo Mali Müşavir Ofisi",
                TaxNo = "2222222222",
                TaxOffice = "Demo Vergi Dairesi",
                Address = "Demo Address",
                IsActive = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        b.Entity<Company>().HasData(
            new Company
            {
                Id = DemoSeedIds.CompanyDemo,
                TenantId = DemoSeedIds.TenantDemo,
                Title = "Demo Mükellef A.Ş.",
                TaxNo = "3333333333",
                TaxOffice = "Demo Vergi Dairesi",
                Email = "company@demo.local",
                Phone = "0000",
                Address = "Demo Company Address",
                IsActive = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );
    }

    private static void SeedBankingAndImport(ModelBuilder b)
    {
        // Banka hesabı ve şablon: Tanıtım.xlsx başlıklarına uygun.
        b.Entity<BankAccount>().HasData(
            new BankAccount
            {
                Id = DemoSeedIds.BankAccountDemo,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = DemoSeedIds.CompanyDemo,
                BankName = "Demo Bank",
                Iban = "TR000000000000000000000000",
                Currency = "TRY",
                AccountingBankAccountCode = "102.01.001",
                AccountingBankAccountName = "Demo Banka Hesabı",
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        var columnMapJson =
            """
            {
              "date": "TARİH",
              "valueDate": "VALÖR",
              "ref": "REF NO",
              "amount": "İŞLEM TUTARI",
              "balance": "BAKİYE",
              "desc": "AÇIKLAMA"
            }
            """;

        b.Entity<BankTemplate>().HasData(
            new BankTemplate
            {
                Id = DemoSeedIds.BankTemplateDemo,
                TenantId = DemoSeedIds.TenantDemo,
                Name = "Demo Ekstre Şablonu (Excel)",
                BankName = "Demo Bank",
                ColumnMapJson = columnMapJson,
                CultureName = "tr-TR",
                AmountNegativeMeansOutflow = true,
                IsActive = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        // Örnek mapping rule: açıklamada POS geçiyorsa gider hesabı öner (sadece çıkış)
        b.Entity<BankMappingRule>().HasData(
            new BankMappingRule
            {
                Id = DemoSeedIds.MappingRuleDemo,
                TenantId = DemoSeedIds.TenantDemo,
                Name = "POS Harcamaları",
                MatchType = Entities.Enums.MatchType.Contains,
                Pattern = "POS",
                CounterAccountCode = "770.01.001",
                CounterAccountName = "Genel Yönetim Giderleri",
                OnlyOutflow = true,
                Priority = 10,
                IsActive = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        // Import'un kaynak dosyası (metadatası)
        b.Entity<DocumentFile>().HasData(
            new DocumentFile
            {
                Id = DemoSeedIds.FileBankStatementDemo,
                TenantId = DemoSeedIds.TenantDemo,
                FileName = "demo-bank-statement.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                SizeBytes = 12345,
                StorageProvider = "local",
                StoragePath = "seed/demo-bank-statement.xlsx",
                Sha256 = null,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        // Import oturumu
        b.Entity<BankStatementImport>().HasData(
            new BankStatementImport
            {
                Id = DemoSeedIds.BankImportDemo,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = DemoSeedIds.CompanyDemo,
                BankAccountId = DemoSeedIds.BankAccountDemo,
                TemplateId = DemoSeedIds.BankTemplateDemo,
                SourceFileId = DemoSeedIds.FileBankStatementDemo,
                Status = BankImportStatus.Mapped,
                TotalRows = 3,
                ImportedRows = 3,
                Notes = "Demo import",
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        // Import satırları (3 satır demo)
        b.Entity<BankTransaction>().HasData(
            new BankTransaction
            {
                Id = DemoSeedIds.Tx1,
                TenantId = DemoSeedIds.TenantDemo,
                ImportId = DemoSeedIds.BankImportDemo,
                TransactionDate = new DateTime(2025, 12, 25),
                ValueDate = new DateTime(2025, 12, 25),
                ReferenceNo = "REF001",
                Description = "POS HARCAMA MARKET",
                Amount = -600000.00m,
                BalanceAfter = 1400000.00m,
                RowNo = 2,
                MappingStatus = MappingStatus.Approved,
                SuggestedCounterAccountCode = "770.01.001",
                ApprovedCounterAccountCode = "770.01.001",
                AppliedRuleId = DemoSeedIds.MappingRuleDemo,
                Confidence = 0.95m,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new BankTransaction
            {
                Id = DemoSeedIds.Tx2,
                TenantId = DemoSeedIds.TenantDemo,
                ImportId = DemoSeedIds.BankImportDemo,
                TransactionDate = new DateTime(2025, 12, 26),
                ValueDate = new DateTime(2025, 12, 26),
                ReferenceNo = "REF002",
                Description = "HAVALE GELİŞİ",
                Amount = 250000.00m,
                BalanceAfter = 1650000.00m,
                RowNo = 3,
                MappingStatus = MappingStatus.Unmapped,
                SuggestedCounterAccountCode = null,
                ApprovedCounterAccountCode = null,
                AppliedRuleId = null,
                Confidence = null,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new BankTransaction
            {
                Id = DemoSeedIds.Tx3,
                TenantId = DemoSeedIds.TenantDemo,
                ImportId = DemoSeedIds.BankImportDemo,
                TransactionDate = new DateTime(2025, 12, 27),
                ValueDate = new DateTime(2025, 12, 27),
                ReferenceNo = "REF003",
                Description = "POS HARCAMA AKARYAKIT",
                Amount = -50000.00m,
                BalanceAfter = 1600000.00m,
                RowNo = 4,
                MappingStatus = MappingStatus.Suggested,
                SuggestedCounterAccountCode = "770.01.001",
                ApprovedCounterAccountCode = null,
                AppliedRuleId = DemoSeedIds.MappingRuleDemo,
                Confidence = 0.80m,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );
    }

    private static void SeedVoucherDraft(ModelBuilder b)
    {
        // Demo olarak fiş taslağı da seed ediyoruz:
        // Böylece UI "Draft" ekranlarını ilk çalıştırmada test edebilirsin.
        b.Entity<VoucherDraft>().HasData(
            new VoucherDraft
            {
                Id = DemoSeedIds.VoucherDraftDemo,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = DemoSeedIds.CompanyDemo,
                ImportId = DemoSeedIds.BankImportDemo,
                VoucherDate = new DateTime(2025, 12, 25),
                Description = "Demo Banka Fişi",
                BankAccountCode = "102.01.001",
                Status = VoucherDraftStatus.Draft,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        // Tx1 (600.000 çıkış) için muhasebe mantığı:
        // Banka (102) ALACAK, Gider (770) BORÇ
        b.Entity<VoucherDraftLine>().HasData(
            new VoucherDraftLine
            {
                Id = DemoSeedIds.VLine1,
                TenantId = DemoSeedIds.TenantDemo,
                VoucherDraftId = DemoSeedIds.VoucherDraftDemo,
                LineNo = 1,
                AccountCode = "770.01.001",
                AccountName = "Genel Yönetim Giderleri",
                Debit = 600000.00m,
                Credit = 0m,
                LineDescription = "POS HARCAMA MARKET",
                CostCenterCode = null,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new VoucherDraftLine
            {
                Id = DemoSeedIds.VLine2,
                TenantId = DemoSeedIds.TenantDemo,
                VoucherDraftId = DemoSeedIds.VoucherDraftDemo,
                LineNo = 2,
                AccountCode = "102.01.001",
                AccountName = "Demo Banka Hesabı",
                Debit = 0m,
                Credit = 600000.00m,
                LineDescription = "POS HARCAMA MARKET",
                CostCenterCode = null,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );

        // İzlenebilirlik: Tx1 hangi satırları üretti?
        b.Entity<VoucherDraftItem>().HasData(
            new VoucherDraftItem
            {
                Id = DemoSeedIds.VItem1,
                TenantId = DemoSeedIds.TenantDemo,
                VoucherDraftId = DemoSeedIds.VoucherDraftDemo,
                BankTransactionId = DemoSeedIds.Tx1,
                FirstLineNo = 1,
                LastLineNo = 2,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );
    }

    private static void SeedMemberships(ModelBuilder b)
    {
        // Identity role yetkilendirmesine ek olarak “scope/tenant” kontrolü için Membership seed ediyoruz.
        b.Entity<UserMembership>().HasData(
            new UserMembership
            {
                Id = DemoSeedIds.MAdmin,
                UserId = DemoSeedIds.UserAdmin,
                Role = MembershipRole.Admin,
                DealerId = null,
                TenantId = null,
                CompanyId = null,
                IsPrimary = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new UserMembership
            {
                Id = DemoSeedIds.MDealer,
                UserId = DemoSeedIds.UserDealer,
                Role = MembershipRole.Dealer,
                DealerId = DemoSeedIds.DealerDemo,
                TenantId = null,
                CompanyId = null,
                IsPrimary = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new UserMembership
            {
                Id = DemoSeedIds.MAccountant,
                UserId = DemoSeedIds.UserAccountant,
                Role = MembershipRole.Accountant,
                DealerId = DemoSeedIds.DealerDemo,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = null,
                IsPrimary = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new UserMembership
            {
                Id = DemoSeedIds.MStaff,
                UserId = DemoSeedIds.UserStaff,
                Role = MembershipRole.AccountantStaff,
                DealerId = null,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = null,
                IsPrimary = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            },
            new UserMembership
            {
                Id = DemoSeedIds.MCompanyUser,
                UserId = DemoSeedIds.UserCompany,
                Role = MembershipRole.CompanyUser,
                DealerId = null,
                TenantId = DemoSeedIds.TenantDemo,
                CompanyId = DemoSeedIds.CompanyDemo,
                IsPrimary = true,
                CreatedAt = SeedTime,
                IsDeleted = false
            }
        );
    }
}
