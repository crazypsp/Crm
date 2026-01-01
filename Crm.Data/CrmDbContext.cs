using Crm.Data.Seeding;
using Crm.Entities.Banking;
using Crm.Entities.Documents;
using Crm.Entities.Identity;
using Crm.Entities.Integration;
using Crm.Entities.Messaging;
using Crm.Entities.Tenancy;
using Crm.Entities.Work;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crm.Data;

public class CrmDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

    // Tenancy
    public DbSet<Dealer> Dealers => Set<Dealer>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyContact> CompanyContacts => Set<CompanyContact>();
    public DbSet<UserMembership> UserMemberships => Set<UserMembership>();

    // Integration
    public DbSet<ConnectionSecret> ConnectionSecrets => Set<ConnectionSecret>();
    public DbSet<IntegrationProfile> IntegrationProfiles => Set<IntegrationProfile>();
    public DbSet<AgentMachine> AgentMachines => Set<AgentMachine>();
    public DbSet<IntegrationJob> IntegrationJobs => Set<IntegrationJob>();

    // Banking
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<BankTemplate> BankTemplates => Set<BankTemplate>();
    public DbSet<BankStatementImport> BankStatementImports => Set<BankStatementImport>();
    public DbSet<BankTransaction> BankTransactions => Set<BankTransaction>();
    public DbSet<BankMappingRule> BankMappingRules => Set<BankMappingRule>();
    public DbSet<VoucherDraft> VoucherDrafts => Set<VoucherDraft>();
    public DbSet<VoucherDraftLine> VoucherDraftLines => Set<VoucherDraftLine>();
    public DbSet<VoucherDraftItem> VoucherDraftItems => Set<VoucherDraftItem>();

    // Work
    public DbSet<WorkTask> WorkTasks => Set<WorkTask>();
    public DbSet<WorkTaskAssignment> WorkTaskAssignments => Set<WorkTaskAssignment>();

    // Documents
    public DbSet<DocumentFile> DocumentFiles => Set<DocumentFile>();
    public DbSet<DocumentRequest> DocumentRequests => Set<DocumentRequest>();
    public DbSet<DocumentRequestItem> DocumentRequestItems => Set<DocumentRequestItem>();
    public DbSet<DocumentSubmission> DocumentSubmissions => Set<DocumentSubmission>();

    // Messaging
    public DbSet<MessageThread> MessageThreads => Set<MessageThread>();
    public DbSet<ThreadParticipant> ThreadParticipants => Set<ThreadParticipant>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // =========================================================
        // 1) Finansal alanlar - muhasebede precision standardı
        // =========================================================
        b.Entity<BankTransaction>().Property(x => x.Amount).HasPrecision(18, 2);
        b.Entity<BankTransaction>().Property(x => x.BalanceAfter).HasPrecision(18, 2);
        b.Entity<VoucherDraftLine>().Property(x => x.Debit).HasPrecision(18, 2);
        b.Entity<VoucherDraftLine>().Property(x => x.Credit).HasPrecision(18, 2);

        // =========================================================
        // 2) Index/unique kuralları
        // =========================================================
        // Import içindeki satır numarası benzersiz (idempotency)
        b.Entity<BankTransaction>()
            .HasIndex(x => new { x.TenantId, x.ImportId, x.RowNo })
            .IsUnique();

        // Banka hesap kodu şirkette benzersiz olsun
        b.Entity<BankAccount>()
            .HasIndex(x => new { x.TenantId, x.CompanyId, x.AccountingBankAccountCode })
            .IsUnique();

        // =========================================================
        // 3) İLİŞKİLER (kritik)
        // =========================================================

        // 3.1 BankAccount -> Company (CompanyId1 shadow FK problemini çözer)
        // Neden: Navigation üzerinden tek ilişkiyi netleştirir, EF'nin ikinci ilişki üretmesini engeller.
        b.Entity<BankAccount>(e =>
        {
            e.HasOne(x => x.Company)
             .WithMany(c => c.BankAccounts)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.NoAction); // soft delete kullandığın için doğru
        });

        // 3.2 BankStatementImport ilişkileri (multiple cascade path riskini kapatır)
        b.Entity<BankStatementImport>(e =>
        {
            e.HasOne(x => x.Company)
             .WithMany()
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.BankAccount)
             .WithMany()
             .HasForeignKey(x => x.BankAccountId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Template)
             .WithMany()
             .HasForeignKey(x => x.TemplateId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.SourceFile)
             .WithMany()
             .HasForeignKey(x => x.SourceFileId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // 3.3 BankTransaction -> Import
        // Neden: Import silinirse transaction’lar cascade ile gitmesin; soft delete tercih.
        b.Entity<BankTransaction>(e =>
        {
            e.HasOne(x => x.Import)
             .WithMany(i => i.Transactions)
             .HasForeignKey(x => x.ImportId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // 3.4 VoucherDraft -> Import
        b.Entity<VoucherDraft>(e =>
        {
            e.HasOne(x => x.Import)
             .WithMany()
             .HasForeignKey(x => x.ImportId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // 3.5 VoucherDraftLine -> VoucherDraft
        b.Entity<VoucherDraftLine>(e =>
        {
            e.HasOne(x => x.VoucherDraft)
             .WithMany(d => d.Lines)
             .HasForeignKey(x => x.VoucherDraftId)
             .OnDelete(DeleteBehavior.NoAction);
        });
        foreach (var fk in b.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
        {
            if (fk.IsOwnership) continue;
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }
        // =========================================================
        // 4) Demo seed
        // =========================================================
        // Öneri: İlk migration'ı seed olmadan üret; sonra seed'i ekleyip ikinci migration üret.
        DemoSeedData.Apply(b);
    }
}
