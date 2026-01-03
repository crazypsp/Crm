using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Api.Shared.Observability
{
    /// <summary>
    /// Neden: Dağıtık mimaride bir isteğin Gateway->API->DB->Agent zincirinde izlenebilmesi gerekir.
    /// CorrelationId ile loglar aynı anahtar altında birleşir.
    /// </summary>
    public sealed class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx)
        {
            var cid = ctx.Request.Headers.TryGetValue(HeaderName, out var existing) && !string.IsNullOrWhiteSpace(existing)
                ? existing.ToString()
                : Guid.NewGuid().ToString("N");

            ctx.Items[HeaderName] = cid;
            ctx.Response.Headers[HeaderName] = cid;

            await _next(ctx);
        }
    }
}
