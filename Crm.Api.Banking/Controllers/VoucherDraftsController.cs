using Crm.Api.Banking.Contracts;
using Crm.Api.Banking.Infrastructure;
using Crm.Data;
using Crm.Entities.Banking;
using Crm.Entities.Contracts.Accounting;
using Crm.Entities.Integration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Crm.Api.Banking.Controllers
{
    [ApiController]
    [Route("api/banking/vouchers")]
    public sealed class VoucherWorkflowController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public VoucherWorkflowController(CrmDbContext db) => _db = db;

        [HttpPost("from-import")]
        public async Task<ActionResult<VoucherDraftDto>> CreateFromImport(
            [FromBody] CreateVoucherDraftFromImportRequest req,
            CancellationToken ct)
        {
            // Neden: Import header olmadan fiş üretilemez.
            var import = await _db.BankStatementImports
                .FirstOrDefaultAsync(x =>
                    x.Id == req.ImportId &&
                    x.TenantId == req.TenantId &&
                    x.CompanyId == req.CompanyId &&
                    !x.IsDeleted, ct);

            if (import is null)
                return NotFound(new { message = "Import bulunamadı." });

            // Neden: Fiş satırlarının hammaddesi import altındaki BankTransaction’lardır.
            var txs = await _db.BankTransactions
                .Where(x => x.TenantId == req.TenantId && x.ImportId == req.ImportId && !x.IsDeleted)
                .OrderBy(x => x.RowNo)
                .ToListAsync(ct);

            if (txs.Count == 0)
                return BadRequest(new { message = "Import içinde satır yok." });

            // Neden: Banka GL hesabı (102/108...) fişte banka tarafı satırında kullanılacak.
            var bankGl = (req.BankGlAccountCodeOverride ?? "").Trim();

            if (string.IsNullOrWhiteSpace(bankGl))
            {
                var bankAccount = await _db.BankAccounts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == import.BankAccountId && x.TenantId == req.TenantId && !x.IsDeleted, ct);

                bankGl = bankAccount?.AccountingBankAccountCode?.Trim() ?? "";
            }

            if (string.IsNullOrWhiteSpace(bankGl))
                return BadRequest(new
                {
                    message = "Banka muhasebe hesap kodu bulunamadı. BankGlAccountCodeOverride gönderin veya BankAccount.AccountingBankAccountCode tanımlayın."
                });

            // Neden: Fiş tarihi kullanıcı seçebilir; seçmezse ilk hareket tarihini alırız.
            var voucherDate = (req.VoucherDate ?? txs.Min(x => x.TransactionDate)).Date;

            // Neden: Mapping rule’lar otomatik karşı hesap önerisi üretmek için.
            // BankMappingRulesController’da aynı filtre yaklaşımı var. :contentReference[oaicite:2]{index=2}
            var rulesQuery = _db.BankMappingRules.AsNoTracking()
                .Where(x => x.TenantId == req.TenantId && !x.IsDeleted);

            rulesQuery = rulesQuery.Where(x => EF.Property<Guid?>(x, "CompanyId") == null || EF.Property<Guid?>(x, "CompanyId") == req.CompanyId);
            rulesQuery = rulesQuery.Where(x => EF.Property<Guid?>(x, "TemplateId") == null || EF.Property<Guid?>(x, "TemplateId") == import.TemplateId);

            var rules = await rulesQuery.ToListAsync(ct);

            // Neden: TryGetBool bool? dönebilir; && hatası almamak için bool’a normalize ediyoruz.
            var compiledRules = rules
                .Select(r => new
                {
                    MatchText = (EntityMap.TryGetString(r, "MatchText", "ContainsText", "Keyword", "DescriptionContains") ?? "").Trim(),
                    Counter = (EntityMap.TryGetString(r, "CounterAccountCode", "CounterCode", "AccountingCounterAccountCode") ?? "").Trim(),
                    Priority = TryInt(EntityMap.TryGet(r, "Priority", "Order")) ?? 0,
                    IsActive = EntityMap.TryGetBool(r, "IsActive") ?? false
                })
                .Where(x => x.IsActive == true && x.MatchText.Length > 0 && x.Counter.Length > 0)
                .OrderByDescending(x => x.Priority)
                .ToList();

            // 1) VoucherDraft entity oluştur
            // Neden: “tek fişin üzerine yazmak” için üst kayıt gerekir.
            var draft = new VoucherDraft
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            EntityMap.TrySet(draft,
                ("CompanyId", req.CompanyId),
                ("ImportId", req.ImportId),
                ("SourceImportId", req.ImportId),
                ("VoucherDate", voucherDate),
                ("Date", voucherDate),
                ("Description", req.HeaderDescription ?? $"Banka Fişi - Import {req.ImportId:N}"),
                ("BankGlAccountCode", bankGl),
                ("BankAccountCode", bankGl),
                ("BankAccountId", import.BankAccountId)
            );

            _db.VoucherDrafts.Add(draft);

            // 2) DraftLine üret
            // Neden: Her banka hareketinden muhasebe mantığı ile 2 satır üretiriz (banka + karşı hesap).
            var lineDtos = new List<VoucherDraftLineDto>();
            var lineNo = 0;

            foreach (var tx in txs)
            {
                if (tx.Amount == 0) continue;

                var abs = Math.Abs(tx.Amount);

                // Neden: Önce kullanıcı onayı; yoksa öneri; yoksa rule ile üret.
                var counter = (tx.ApprovedCounterAccountCode ?? "").Trim();

                if (string.IsNullOrWhiteSpace(counter))
                {
                    counter = (tx.SuggestedCounterAccountCode ?? "").Trim();
                }

                if (string.IsNullOrWhiteSpace(counter))
                {
                    var hit = compiledRules.FirstOrDefault(r =>
                        (tx.Description ?? "").IndexOf(r.MatchText, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (hit is not null)
                    {
                        counter = hit.Counter;
                        tx.SuggestedCounterAccountCode = counter; // UI’da tekrar gösterilsin
                    }
                }

                if (string.IsNullOrWhiteSpace(counter))
                {
                    if (req.FailIfUnmapped)
                        return BadRequest(new { message = $"Karşı hesap bulunamadı. RowNo={tx.RowNo}, Desc='{tx.Description}'" });

                    counter = req.SuspenseAccountCode.Trim();
                }

                if (string.IsNullOrWhiteSpace(counter))
                    return BadRequest(new { message = "SuspenseAccountCode boş olamaz." });

                // Amount > 0 (giriş): Banka BORÇ, Karşı ALACAK
                // Amount < 0 (çıkış): Karşı BORÇ, Banka ALACAK
                if (tx.Amount > 0)
                {
                    // Banka BORÇ
                    lineNo++;
                    _db.VoucherDraftLines.Add(BuildLine(draft.Id, req.TenantId, lineNo, bankGl, abs, 0m, tx.Description, tx.Id));
                    lineDtos.Add(new VoucherDraftLineDto(lineNo, bankGl, abs, 0m, tx.Description, tx.Id.ToString()));

                    // Karşı ALACAK
                    lineNo++;
                    _db.VoucherDraftLines.Add(BuildLine(draft.Id, req.TenantId, lineNo, counter, 0m, abs, tx.Description, tx.Id));
                    lineDtos.Add(new VoucherDraftLineDto(lineNo, counter, 0m, abs, tx.Description, tx.Id.ToString()));
                }
                else
                {
                    // Karşı BORÇ
                    lineNo++;
                    _db.VoucherDraftLines.Add(BuildLine(draft.Id, req.TenantId, lineNo, counter, abs, 0m, tx.Description, tx.Id));
                    lineDtos.Add(new VoucherDraftLineDto(lineNo, counter, abs, 0m, tx.Description, tx.Id.ToString()));

                    // Banka ALACAK
                    lineNo++;
                    _db.VoucherDraftLines.Add(BuildLine(draft.Id, req.TenantId, lineNo, bankGl, 0m, abs, tx.Description, tx.Id));
                    lineDtos.Add(new VoucherDraftLineDto(lineNo, bankGl, 0m, abs, tx.Description, tx.Id.ToString()));
                }
            }

            await _db.SaveChangesAsync(ct);

            // DİKKAT: Senin VoucherDraftDto’n record ve ctor istiyor.
            // Hata aldığın yer burasıydı; property set etmiyoruz, ctor ile üretiyoruz.
            var headerDesc = (EntityMap.TryGetString(draft, "Description") ?? "").Trim();
            return Ok(new VoucherDraftDto(draft.Id, voucherDate, headerDesc, bankGl, lineDtos));

            static VoucherDraftLine BuildLine(Guid draftId, Guid tenantId, int lineNo, string accountCode, decimal debit, decimal credit, string? desc, Guid txId)
            {
                var line = new VoucherDraftLine
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };

                // Neden: Entity alan isimleri projede değişebilir; güvenli set.
                EntityMap.TrySet(line,
                    ("VoucherDraftId", draftId),
                    ("DraftId", draftId),
                    ("LineNo", lineNo),
                    ("AccountCode", accountCode),
                    ("AccountingAccountCode", accountCode),
                    ("Debit", debit),
                    ("Credit", credit),
                    ("Description", desc),
                    ("SourceTransactionId", txId),
                    ("BankTransactionId", txId)
                );

                return line;
            }

            static int? TryInt(object? v) => v is null ? null : (int.TryParse(v.ToString(), out var i) ? i : null);
        }

        [HttpPut("transactions/{transactionId:guid}/approve")]
        public async Task<IActionResult> ApproveCounterAccount(
            Guid transactionId,
            [FromBody] ApproveTransactionCounterAccountRequest req,
            CancellationToken ct)
        {
            // Neden: Otomatik kural tutmadığında satır bazında manuel onay şart.
            var tx = await _db.BankTransactions
                .FirstOrDefaultAsync(x => x.Id == transactionId && x.TenantId == req.TenantId && !x.IsDeleted, ct);

            if (tx is null) return NotFound();

            tx.ApprovedCounterAccountCode = req.ApprovedCounterAccountCode.Trim();
            tx.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpPost("{voucherDraftId:guid}/enqueue")]
        public async Task<ActionResult<EnqueuePostVoucherResponse>> Enqueue(
            Guid voucherDraftId,
            [FromBody] EnqueuePostVoucherRequest req,
            CancellationToken ct)
        {
            // Neden: ERP’ye yazma Agent tarafından yapılır; Banking burada iş (job) üretir.
            var exists = await _db.VoucherDrafts.AsNoTracking()
                .AnyAsync(x => x.Id == voucherDraftId && x.TenantId == req.TenantId && !x.IsDeleted, ct);

            if (!exists) return NotFound();

            var payload = JsonSerializer.Serialize(new
            {
                tenantId = req.TenantId,
                voucherDraftId,
                integrationProfileId = req.IntegrationProfileId
            });

            var job = new IntegrationJob
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            // Neden: Alanlar projede farklı adlandırılmış olabilir; TrySet ile minimum zorunlu alanlar doldurulur.
            EntityMap.TrySet(job,
                ("IntegrationProfileId", req.IntegrationProfileId),
                ("JobType", "PostVoucherDraft"),
                ("Type", "PostVoucherDraft"),
                ("PayloadJson", payload),
                ("DataJson", payload),
                ("Status", 0) // MVP: 0 = Queued varsayımı
            );

            _db.IntegrationJobs.Add(job);
            await _db.SaveChangesAsync(ct);

            return Ok(new EnqueuePostVoucherResponse { JobId = job.Id });
        }
    }
}
