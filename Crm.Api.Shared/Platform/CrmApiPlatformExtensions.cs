using System.Text;
using System.Threading.RateLimiting;
using Crm.Api.Shared.Observability;
using Crm.Api.Shared.Security;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Crm.Api.Shared.Platform;

/// <summary>
/// Neden: 10 ayrı API projesinde aynı auth/validation/logging/health/swagger/rate-limit kurulumunu
/// copy-paste yapmak sürdürülebilir değil. Bu extension'lar tek standardı garanti eder.
/// </summary>
public static class CrmApiPlatformExtensions
{
    /// <summary>
    /// Neden: Servis kayıtlarını standardize eder (AuthN/AuthZ, Validation, ProblemDetails, Health, Swagger, CORS, RateLimit).
    /// Her API projesi bunu çağırarak aynı davranışı kazanır.
    /// </summary>
    public static IServiceCollection AddCrmApiPlatform(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();

        // Current user context (TenantId gibi scope değerlerini JWT claim'den okumak için)
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();

        // MVC + tek tip validation response
        services.AddControllers()
            .ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = ctx =>
                {
                    // Neden: UI tarafında tek formatla hata yakalamak (ProblemDetails).
                    var pd = new ValidationProblemDetails(ctx.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed"
                    };
                    return new BadRequestObjectResult(pd);
                };
            });

        // FluentValidation auto pipeline
        services.AddFluentValidationAutoValidation();

        // ProblemDetails (exception -> standard response)
        services.AddProblemDetails(opt =>
        {
            // Neden: Prod ortamında exception detail sızdırmayız.
            opt.IncludeExceptionDetails = (_, __) => false;

            // Tipik exception mapping'leri
            opt.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            opt.MapToStatusCode<ArgumentException>(StatusCodes.Status400BadRequest);
        });

        // AuthN: JWT
        var jwt = config.GetSection("Auth:Jwt");
        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var key = jwt["Key"];

        if (string.IsNullOrWhiteSpace(key) || key.Length < 32)
            throw new InvalidOperationException("Auth:Jwt:Key missing or too short. Use >= 32 chars. Use secrets/env in production.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        // AuthZ: Roles/Policies
        services.AddAuthorization(o =>
        {
            o.AddPolicy(CrmPolicies.AdminOnly, p => p.RequireRole(CrmRoles.Admin));
            o.AddPolicy(CrmPolicies.DealerOnly, p => p.RequireRole(CrmRoles.Dealer));
            o.AddPolicy(CrmPolicies.AccountantOnly, p => p.RequireRole(CrmRoles.Accountant));
            o.AddPolicy(CrmPolicies.StaffOnly, p => p.RequireRole(CrmRoles.AccountantStaff));
            o.AddPolicy(CrmPolicies.CompanyOnly, p => p.RequireRole(CrmRoles.CompanyUser));
        });

        // API Versioning (v1)
        services.AddApiVersioning(o =>
        {
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ReportApiVersions = true;
        });

        // CORS (MVP: herkes; Prod: whitelist)
        services.AddCors(o =>
        {
            o.AddPolicy("DefaultCors", p =>
            {
                p.AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials()
                 .SetIsOriginAllowed(_ => true); // Prod'da origin whitelist önerilir
            });
        });

        // Rate limiting (IP bazlı)
        services.AddRateLimiter(o =>
        {
            o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
            {
                var key = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 120,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
            });
        });

        // Swagger + JWT
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Health checks (DB connection string verilirse SQL check ekler)
        var cs = config.GetConnectionString("CrmDb");
        var hc = services.AddHealthChecks();

        if (!string.IsNullOrWhiteSpace(cs))
        {
            hc.AddSqlServer(
                cs,
                name: "sql",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "ready" });
        }

        return services;
    }

    /// <summary>
    /// Neden: Pipeline standardı (ProblemDetails, Auth, CORS, RateLimit, Swagger, Health).
    /// Her API aynı sırayla middleware çalıştırır.
    /// </summary>
    public static WebApplication UseCrmApiPlatform(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseProblemDetails();

        app.UseSerilogRequestLogging();

        app.UseCors("DefaultCors");

        app.UseRateLimiter();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        // Health endpoints
        app.MapHealthChecks("/health/live");
        app.MapHealthChecks("/health/ready");

        return app;
    }
}
