using System.Linq.Expressions;
using Crm.Data.Seeding;
using Crm.Entities.Banking;
using Crm.Entities.Common;
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
        // 2) Index/unique kuralları (performans + idempotency)
        // =========================================================
        b.Entity<BankTransaction>()
            .HasIndex(x => new { x.TenantId, x.ImportId, x.RowNo })
            .IsUnique();

        b.Entity<BankAccount>()
            .HasIndex(x => new { x.TenantId, x.CompanyId, x.AccountingBankAccountCode })
            .IsUnique();

        b.Entity<BankMappingRule>()
            .HasIndex(x => new { x.TenantId, x.CompanyId, x.ProgramType, x.IsActive, x.Priority });

        b.Entity<IntegrationProfile>()
            .HasIndex(x => new { x.TenantId, x.CompanyId, x.ProgramType, x.IsActive });

        // =========================================================
        // 3) İLİŞKİLER (kritik: shadow FK ve cascade path hatalarını önler)
        // =========================================================

        // 3.0 Dealer hiyerarşisi (Bayi → Alt bayi)
        b.Entity<Dealer>(e =>
        {
            e.HasOne(x => x.ParentDealer)
                .WithMany(x => x.SubDealers)
                .HasForeignKey(x => x.ParentDealerId)
                .IsRequired(false)  // Nullable olduğu için
                .OnDelete(DeleteBehavior.NoAction);
        });

        // 3.0.1 Tenant -> Dealer
        b.Entity<Tenant>(e =>
        {
            e.HasOne(x => x.Dealer)
             .WithMany(x => x.Tenants)
             .HasForeignKey(x => x.DealerId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // 3.0.2 Company -> Tenant (Company sınıfında Tenant navigation olmadığı için FK’yı netliyoruz)
        b.Entity<Company>()
            .HasOne<Tenant>()
            .WithMany(t => t.Companies)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        // 3.1 BankAccount -> Company (CompanyId1 shadow FK problemini engeller)
        b.Entity<BankAccount>(e =>
        {
            e.HasOne(x => x.Company)
             .WithMany(c => c.BankAccounts)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // 3.1.1 IntegrationProfile ilişkileri (Company + Secret)
        b.Entity<IntegrationProfile>(e =>
        {
            e.HasOne(x => x.Company)
             .WithMany(c => c.IntegrationProfiles)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Secret)
             .WithMany()
             .HasForeignKey(x => x.SecretId)
             .OnDelete(DeleteBehavior.NoAction);
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

        // 3.3 BankTransaction -> Import (soft delete için NoAction)
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

        // 3.6 VoucherDraftItem -> VoucherDraft + BankTransaction (izlenebilirlik)
        b.Entity<VoucherDraftItem>(e =>
        {
            e.HasOne(x => x.VoucherDraft)
             .WithMany(d => d.Items)
             .HasForeignKey(x => x.VoucherDraftId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.BankTransaction)
             .WithMany()
             .HasForeignKey(x => x.BankTransactionId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // =========================================================
        // 4) Global: Cascade delete kapat (SQL Server "multiple cascade path" engeli)
        // =========================================================
        foreach (var fk in b.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
        {
            if (fk.IsOwnership) continue;
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }

        // =========================================================
        // 5) Soft delete query filter (IsDeleted=true olan kayıtlar otomatik gizlenir)
        // =========================================================
        ApplySoftDeleteQueryFilter(b);

        // =========================================================
        // 6) Demo seed (migration sırasında InsertData üretir)
        // =========================================================
        DemoSeedData.Apply(b);
    }

    private static void ApplySoftDeleteQueryFilter(ModelBuilder b)
    {
        // Neden: UI/API katmanında her sorguda "IsDeleted = 0" yazmak zorunda kalmayalım.
        // Dikkat: Admin ekranında "silinenleri göster" gibi senaryolarda IgnoreQueryFilters kullanılacak.
        foreach (var entityType in b.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (!typeof(BaseEntity).IsAssignableFrom(clrType)) continue;

            var parameter = Expression.Parameter(clrType, "e");
            var prop = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var body = Expression.Equal(prop, Expression.Constant(false));
            var lambda = Expression.Lambda(body, parameter);

            entityType.SetQueryFilter(lambda);
        }
    }
}
