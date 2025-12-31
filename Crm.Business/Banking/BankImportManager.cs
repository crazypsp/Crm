using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Banking;
using Crm.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Banking
{
    public sealed class BankImportManager : IBankImportManager
    {
        private readonly CrmDbContext _db;
        private readonly IBankMappingEngine _engine;
        private readonly IVoucherDraftBuilder _draftBuilder;

        public BankImportManager(CrmDbContext db, IBankMappingEngine engine, IVoucherDraftBuilder draftBuilder)
        {
            _db = db;
            _engine = engine;
            _draftBuilder = draftBuilder;
        }

        public async Task<BankStatementImport> CreateImportAsync(
            Guid tenantId, Guid companyId, Guid bankAccountId, Guid templateId, Guid sourceFileId, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(companyId, nameof(companyId));
            Guard.NotEmpty(bankAccountId, nameof(bankAccountId));
            Guard.NotEmpty(templateId, nameof(templateId));
            Guard.NotEmpty(sourceFileId, nameof(sourceFileId));

            // Yetki/scope: Company ve BankAccount aynı tenant altında mı?
            var companyOk = await _db.Companies.AnyAsync(x => x.Id == companyId && x.TenantId == tenantId && !x.IsDeleted, ct);
            if (!companyOk) throw new ForbiddenException("Firma bulunamadı veya tenant yetkisi yok.");

            var bankOk = await _db.BankAccounts.AnyAsync(x => x.Id == bankAccountId && x.TenantId == tenantId && x.CompanyId == companyId && !x.IsDeleted, ct);
            if (!bankOk) throw new ForbiddenException("Banka hesabı bulunamadı veya firmaya/tenant’a ait değil.");

            var templateOk = await _db.BankTemplates.AnyAsync(x => x.Id == templateId && x.TenantId == tenantId && x.IsActive && !x.IsDeleted, ct);
            if (!templateOk) throw new ForbiddenException("Banka şablonu bulunamadı veya pasif.");

            var import = new BankStatementImport
            {
                TenantId = tenantId,
                CompanyId = companyId,
                BankAccountId = bankAccountId,
                TemplateId = templateId,
                SourceFileId = sourceFileId,
                Status = BankImportStatus.Uploaded
            };

            _db.BankStatementImports.Add(import);
            await _db.SaveChangesAsync(ct);
            return import;
        }

        public async Task AddTransactionsAsync(Guid tenantId, Guid importId, IReadOnlyList<BankTransaction> transactions, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(importId, nameof(importId));
            Guard.NotNull(transactions, nameof(transactions));

            var import = await _db.BankStatementImports
                .FirstOrDefaultAsync(x => x.Id == importId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Import bulunamadı.");

            if (import.Status is not (BankImportStatus.Uploaded or BankImportStatus.Extracted))
                throw new ValidationException("Bu import durumunda satır eklenemez.");

            foreach (var tx in transactions)
            {
                tx.TenantId = tenantId;
                tx.ImportId = importId;
            }

            _db.BankTransactions.AddRange(transactions);

            import.TotalRows = transactions.Count;
            import.ImportedRows = transactions.Count;
            import.Status = BankImportStatus.Normalized;

            await _db.SaveChangesAsync(ct);
        }

        public async Task ApplyMappingRulesAsync(Guid tenantId, Guid importId, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(importId, nameof(importId));

            var import = await _db.BankStatementImports
                .FirstOrDefaultAsync(x => x.Id == importId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Import bulunamadı.");

            var rules = await _db.BankMappingRules
                .Where(r => r.TenantId == tenantId && r.IsActive && !r.IsDeleted)
                .OrderBy(r => r.Priority)
                .ToListAsync(ct);

            var txs = await _db.BankTransactions
                .Where(t => t.TenantId == tenantId && t.ImportId == importId && !t.IsDeleted)
                .ToListAsync(ct);

            _engine.ApplyRules(rules, txs);

            import.Status = BankImportStatus.Mapped;
            await _db.SaveChangesAsync(ct);
        }

        public async Task ApproveTransactionMappingAsync(Guid tenantId, Guid transactionId, string counterAccountCode, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(transactionId, nameof(transactionId));
            Guard.NotBlank(counterAccountCode, nameof(counterAccountCode));

            var tx = await _db.BankTransactions
                .FirstOrDefaultAsync(x => x.Id == transactionId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Banka hareketi bulunamadı.");

            tx.ApprovedCounterAccountCode = counterAccountCode.Trim();
            tx.MappingStatus = MappingStatus.Approved;
            await _db.SaveChangesAsync(ct);
        }

        public async Task<VoucherDraft> BuildVoucherDraftAsync(Guid tenantId, Guid importId, string bankAccountCode, CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(importId, nameof(importId));
            Guard.NotBlank(bankAccountCode, nameof(bankAccountCode));

            var import = await _db.BankStatementImports
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == importId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Import bulunamadı.");

            var txs = await _db.BankTransactions
                .Where(t => t.TenantId == tenantId && t.ImportId == importId && !t.IsDeleted)
                .ToListAsync(ct);

            var draft = _draftBuilder.Build(tenantId, import.CompanyId, importId, bankAccountCode, txs);

            _db.VoucherDrafts.Add(draft);

            // Status geçişi iş kuralıdır
            var importTracked = await _db.BankStatementImports.FirstAsync(x => x.Id == importId && x.TenantId == tenantId, ct);
            importTracked.Status = BankImportStatus.DraftCreated;

            await _db.SaveChangesAsync(ct);
            return draft;
        }
    }
}
