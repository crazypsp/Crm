using Crm.Business.Tenancy;
using Crm.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Crm.Api.Admin.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAdminApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<CrmDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("AdminConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("Crm.Data");
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),null);
                    });

                options.EnableSensitiveDataLogging(
                    configuration.GetValue<bool>("Logging:EnableSensitiveData", false));
            });

            // Business Layer Services
            services.AddScoped<ITenancyManager, TenancyManager>();

            // Controllers
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "CRM Admin API",
                    Version = "v1",
                    Description = "Sistem yönetimi ve yönetici işlemleri API'si"
                });

                c.AddSecurityDefinition("Bearer", new()
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new()
                {
                    {
                        new() { Reference = new()
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AdminApiPolicy", policy =>
                {
                    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                        ?? new[] { "http://localhost:3000", "http://localhost:4200" };

                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Health Checks
            services.AddHealthChecks()
                .AddDbContextCheck<CrmDbContext>();

            return services;
        }
    }
}