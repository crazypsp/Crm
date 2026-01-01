using Crm.Api.Gateway.Contracts.Banking;
using Crm.Services.Integration;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/integration")]
    public class IntegrationDispatchController : ControllerBase
    {
        private readonly IIntegrationDispatcher _dispatcher;

        public IntegrationDispatchController(IIntegrationDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        // Draft'ı muhasebe programına yazmak üzere job oluşturur
        [HttpPost("dispatch-voucher")]
        public async Task<IActionResult> DispatchVoucher([FromBody] DispatchVoucherRequest req, CancellationToken ct)
        {
            var jobId = await _dispatcher.EnqueuePostVoucherAsync(
                req.TenantId,
                req.CompanyId,
                req.IntegrationProfileId,
                req.VoucherDraftId,
                ct);

            return Ok(new { jobId });
        }
    }
}
