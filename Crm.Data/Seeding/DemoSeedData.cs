using System.Text.Json;
using Crm.Entities.Banking;
using Crm.Entities.Documents;
using Crm.Entities.Enums;
using Crm.Entities.Identity;
using Crm.Entities.Tenancy;
using Crm.Entities.Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crm.Data.Seeding;

public static class DemoSeedData
{
    public static void Apply(ModelBuilder b)
    {

        // Not: HasData ile seed -> migration üretilirken InsertData/UpdateData olarak yazılır.
        // Bu yüzden burada "deterministik" alanları mümkün olduğunca sabit tutuyoruz.
        var now = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);

        // ==========================
        // 1) Tenancy: Dealer/Tenant/Company
        // ==========================
        b.Entity<Dealer>().HasData(new Dealer
        {
            Id = DemoSeedIds.Dealer1,
            TenantId = DemoSeedIds.Tenant1, // Dealer da TenantEntity ise; değilse bu satırı kaldır.
            Name = "Demo Bayi",
            CreatedAt = now,
            IsDeleted = false
        });

        b.Entity<Tenant>().HasData(new Tenant
        {
            Id = DemoSeedIds.Tenant1,
            DealerId = DemoSeedIds.Dealer1,
            Title = "Demo Mali Müşavir Ofisi",
            CreatedAt = now,
            IsDeleted = false
        });

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
                CreatedAt = now,
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
                CreatedAt = now,
                IsDeleted = false
            }
        );

        // ==========================
        // 2) Identity: Role + User (basit demo)
        // ==========================
        b.Entity<ApplicationRole>().HasData(
            new ApplicationRole { Id = DemoSeedIds.AdminRole, Name = "Admin", NormalizedName = "ADMIN" },
            new ApplicationRole { Id = DemoSeedIds.DealerRole, Name = "Dealer", NormalizedName = "DEALER" },
            new ApplicationRole { Id = DemoSeedIds.AccountantRole, Name = "Accountant", NormalizedName = "ACCOUNTANT" },
            new ApplicationRole { Id = DemoSeedIds.StaffRole, Name = "Staff", NormalizedName = "STAFF" },
            new ApplicationRole { Id = DemoSeedIds.CompanyRole, Name = "Company", NormalizedName = "COMPANY" }
        );

        // Demo kullanıcı parolaları (dev): "P@ssw0rd!"
        var hasher = new PasswordHasher<ApplicationUser>();

        var admin = new ApplicationUser
        {
            Id = DemoSeedIds.AdminUser,
            UserName = "admin@demo.local",
            NormalizedUserName = "ADMIN@DEMO.LOCAL",
            Email = "admin@demo.local",
            NormalizedEmail = "ADMIN@DEMO.LOCAL",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            CreatedAt = now,
            IsDeleted = false
        };
        admin.PasswordHash = hasher.HashPassword(admin, "P@ssw0rd!");

        b.Entity<ApplicationUser>().HasData(admin);

        // Diğer demo user’ları eklemek istersen aynı mantıkla ekleyebilirsin.
        // (Rol eşlemesi için IdentityUserRole<Guid> HasData da yapılabilir.)

        // ==========================
        // 3) Documents: 2 adet demo dosya
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
                CreatedAt = now,
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
                CreatedAt = now,
                IsDeleted = false
            }
        );

        // ==========================
        // 4) Banking: BankAccount + Template
        // ==========================
        b.Entity<BankAccount>().HasData(new BankAccount
        {
            Id = DemoSeedIds.BankAccount1,
            TenantId = DemoSeedIds.Tenant1,
            CompanyId = DemoSeedIds.Company1,
            BankName = "Demo Bank",
            Iban = "TR000000000000000000000001",
            Currency = "TRY",
            AccountingBankAccountCode = "102.01.001",
            AccountingBankAccountName = "Demo Bank TL",
            CreatedAt = now,
            IsDeleted = false
        });

        var columnMap = new
        {
            date = "İşlem Tarihi",
            desc = "Açıklama",
            amount = "Tutar",
            balance = "Bakiye"
        };

        b.Entity<BankTemplate>().HasData(new BankTemplate
        {
            Id = DemoSeedIds.BankTemplate1,
            TenantId = DemoSeedIds.Tenant1,
            Name = "Demo Ekstre Şablonu",
            BankName = "Demo Bank",
            ColumnMapJson = JsonSerializer.Serialize(columnMap),
            CultureName = "tr-TR",
            AmountNegativeMeansOutflow = true,
            IsActive = true,
            CreatedAt = now,
            IsDeleted = false
        });

        // ==========================
        // 5) Banking: Mapping Rules (açıklamadan karşı hesap önerisi)
        // ==========================
        b.Entity<BankMappingRule>().HasData(
            new BankMappingRule
            {
                Id = DemoSeedIds.BankRule1,
                TenantId = DemoSeedIds.Tenant1,
                Name = "SGK Ödemesi",
                MatchType = MatchType.Contains,
                Pattern = "SGK",
                ProgramType = null,
                CompanyId = DemoSeedIds.Company1,
                CounterAccountCode = "361.01.001",
                CounterAccountName = "SGK Prim Borçları",
                OnlyOutflow = true,
                Priority = 10,
                IsActive = true,
                CreatedAt = now,
                IsDeleted = false
            },
            new BankMappingRule
            {
                Id = DemoSeedIds.BankRule2,
                TenantId = DemoSeedIds.Tenant1,
                Name = "Kira",
                MatchType = MatchType.Contains,
                Pattern = "KİRA",
                ProgramType = null,
                CompanyId = DemoSeedIds.Company1,
                CounterAccountCode = "770.01.001",
                CounterAccountName = "Kira Giderleri",
                OnlyOutflow = true,
                Priority = 20,
                IsActive = true,
                CreatedAt = now,
                IsDeleted = false
            }
        );

        // ==========================
        // 6) Import demo: BankStatementImport + 3 satır transaction
        // ==========================
        b.Entity<BankStatementImport>().HasData(new BankStatementImport
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
            CreatedAt = now,
            IsDeleted = false
        });

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
                CreatedAt = now,
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
                CreatedAt = now,
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
                CreatedAt = now,
                IsDeleted = false
            }
        );

        // ==========================
        // 7) Demo VoucherDraft (tek fiş taslağı örneği)
        // ==========================
        b.Entity<VoucherDraft>().HasData(new VoucherDraft
        {
            Id = DemoSeedIds.VoucherDraft1,
            TenantId = DemoSeedIds.Tenant1,
            CompanyId = DemoSeedIds.Company1,
            ImportId = DemoSeedIds.BankStatementImport1,
            VoucherDate = new DateTime(2026, 01, 02),
            Description = "Demo fiş taslağı (seed)",
            BankAccountCode = "102.01.001",
            Status = VoucherDraftStatus.Draft,
            CreatedAt = now,
            IsDeleted = false
        });

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
                CreatedAt = now,
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
                CreatedAt = now,
                IsDeleted = false
            }
        );

        b.Entity<VoucherDraftItem>().HasData(new VoucherDraftItem
        {
            Id = DemoSeedIds.VoucherItem1,
            TenantId = DemoSeedIds.Tenant1,
            VoucherDraftId = DemoSeedIds.VoucherDraft1,
            BankTransactionId = DemoSeedIds.BankTx1,
            FirstLineNo = 1,
            LastLineNo = 2,
            CreatedAt = now,
            IsDeleted = false
        });

        // ==========================
        // 8) Work demo
        // ==========================
        b.Entity<WorkTask>().HasData(new WorkTask
        {
            Id = DemoSeedIds.Task1,
            TenantId = DemoSeedIds.Tenant1,
            CompanyId = DemoSeedIds.Company1,
            Title = "Demo görev: Ocak KDV kontrolü",
            Description = "Demo seed görev açıklaması",
            Status = WorkTaskStatus.Open,
            Priority = WorkTaskPriority.Normal,
            DueDate = new DateTime(2026, 01, 31),
            CreatedAt = now,
            IsDeleted = false
        });

        // Messaging seed (minimum)
        // Not: MessageThread/Participant/Message seedlerini önceki dosyan varsa aynı şekilde koruyabilirsin.
    }
}
