using Crm.Business.Integration;
using Crm.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Crm.Services.Integration
{
    public sealed class IntegrationDispatcher : IIntegrationDispatcher
    {
        private readonly CrmDbContext _db;
        private readonly IIntegrationJobManager _jobManager;

        public IntegrationDispatcher(CrmDbContext db, IIntegrationJobManager jobManager)
        {
            _db = db;
            _jobManager = jobManager;
        }

        public async Task<Guid> EnqueuePostVoucherAsync(Guid tenantId, Guid companyId, Guid integrationProfileId, Guid voucherDraftId, CancellationToken ct)
        {
            // Draft’ı DB’den al, Agent’a gidecek payload’ı oluştur
            var draft = await _db.VoucherDrafts
                .Include(x => x.Lines)
                .FirstOrDefaultAsync(x => x.Id == voucherDraftId && x.TenantId == tenantId, ct);

            if (draft is null) throw new Exception("Voucher draft bulunamadı.");

            var payload = new
            {
                IntegrationProfileId = integrationProfileId,
                Draft = new
                {
                    VoucherDraftId = draft.Id,
                    draft.VoucherDate,
                    draft.Description,
                    draft.BankAccountCode,
                    Lines = draft.Lines.Select(l => new
                    {
                        l.LineNo,
                        l.AccountCode,
                        l.Debit,
                        l.Credit,
                        l.LineDescription,
                        l.CostCenterCode
                    }).ToList()
                }
            };

            var json = JsonSerializer.Serialize(payload);

            var job = await _jobManager.EnqueueJobAsync(
                tenantId,
                companyId,
                agentMachineId: null,
                commandType: "PostVoucher",
                payloadJson: json,
                ct);

            return job.Id;
        }
    }
}
