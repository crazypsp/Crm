using Crm.Api.Gateway.Contracts.Banking;
using Crm.Business.Banking;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/bank/transactions")]
    public class BankTransactionsController : ControllerBase
    {
        private readonly IBankImportManager _mgr;

        public BankTransactionsController(IBankImportManager mgr) => _mgr = mgr;

        // Kullanıcı tek satır için karşı hesabı onaylar
        [HttpPost("{transactionId:guid}/approve")]
        public async Task<IActionResult> Approve(Guid transactionId, [FromBody] ApproveTransactionRequest req, CancellationToken ct)
        {
            await _mgr.ApproveTransactionMappingAsync(req.TenantId, transactionId, req.CounterAccountCode, ct);
            return Ok(new { ok = true });
        }
    }
}
