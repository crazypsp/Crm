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

        // Finansal alanlar - muhasebede precision standardı
        b.Entity<BankTransaction>().Property(x => x.Amount).HasPrecision(18, 2);
        b.Entity<BankTransaction>().Property(x => x.BalanceAfter).HasPrecision(18, 2);
        b.Entity<VoucherDraftLine>().Property(x => x.Debit).HasPrecision(18, 2);
        b.Entity<VoucherDraftLine>().Property(x => x.Credit).HasPrecision(18, 2);

        // İdempotency & performans: import içindeki satır numarası benzersiz
        b.Entity<BankTransaction>()
            .HasIndex(x => new { x.TenantId, x.ImportId, x.RowNo })
            .IsUnique();

        // Banka hesap kodu şirkette benzersiz olsun
        b.Entity<BankAccount>()
            .HasIndex(x => new { x.TenantId, x.CompanyId, x.AccountingBankAccountCode })
            .IsUnique();

        // Demo seed (migration sırasında InsertData üretmek için HasData kullanıyoruz)
        // Öneri: InitialCreate migration'ını önce seed olmadan üret, sonra bu satırı ekle, AddDemoSeed migration üret.
        DemoSeedData.Apply(b);
    }
}
