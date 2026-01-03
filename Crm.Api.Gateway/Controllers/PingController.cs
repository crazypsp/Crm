using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/gateway/ping")]
    public sealed class PingController : ControllerBase
    {
        /// <summary>
        /// Neden: Gateway ayakta mı? Health dışında "app pipeline" doğrulamak için.
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult Public() => Ok(new { ok = true, mode = "public" });

        /// <summary>
        /// Neden: JWT doğrulaması ve policy zinciri çalışıyor mu test etmek için.
        /// JWT yoksa 401 döner.
        /// </summary>
        [HttpGet("secure")]
        [Authorize]
        public IActionResult Secure() => Ok(new { ok = true, mode = "secure", user = User?.Identity?.Name });
    }
}
