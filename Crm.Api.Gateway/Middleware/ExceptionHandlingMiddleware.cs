using Crm.Business.Common;
using Crm.Services.Common;
using System.Net;
using System.Text.Json;

namespace Crm.Api.Gateway.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteProblemDetails(context, ex);
            }
        }

        private static Task WriteProblemDetails(HttpContext ctx, Exception ex)
        {
            var (status, title) = ex switch
            {
                NotFoundException => ((int)HttpStatusCode.NotFound, "Not Found"),
                ForbiddenException => ((int)HttpStatusCode.Forbidden, "Forbidden"),
                ValidationException => ((int)HttpStatusCode.BadRequest, "Validation Error"),
                UnsupportedFileException => ((int)HttpStatusCode.UnsupportedMediaType, "Unsupported Media Type"),
                ParseException => ((int)HttpStatusCode.BadRequest, "Parse Error"),
                StorageException => ((int)HttpStatusCode.InternalServerError, "Storage Error"),
                _ => ((int)HttpStatusCode.InternalServerError, "Server Error")
            };

            var body = new
            {
                type = "about:blank",
                title,
                status,
                detail = ex.Message,
                traceId = ctx.TraceIdentifier
            };

            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/problem+json";
            return ctx.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
