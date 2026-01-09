using Crm.Business.Banking;
using Crm.Business.Common;
using Crm.Data;
using Crm.Services.Banking;
using Crm.Services.Common;
using Crm.Services.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json;

namespace Crm.Api.Banking.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBankingApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<CrmDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("Crm.Data");
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                options.EnableSensitiveDataLogging(configuration.GetValue<bool>("Logging:EnableSensitiveData"));
            });

            // Business Layer Services
            services.AddScoped<IBankImportManager, BankImportManager>();
            services.AddScoped<IBankMappingEngine, BankMappingEngine>();
            services.AddScoped<IVoucherDraftBuilder, VoucherDraftBuilder>();

            // Service Layer Services
            services.AddScoped<IBankImportAppService, BankImportAppService>();
            services.AddScoped<IBankStatementNormalizer, BankStatementNormalizer>();

            // Statement Extractors
            services.AddScoped<ExcelStatementExtractor>();
            services.AddScoped<PdfStatementExtractor>();
            services.AddScoped<IStatementExtractor>(provider =>
            {
                // Burada file extension'a göre extractor seçilebilir
                // Şimdilik Excel döndürüyoruz
                return provider.GetRequiredService<ExcelStatementExtractor>();
            });

            // Common Services
            services.AddScoped<IClock, SystemClock>();
            services.AddScoped<IFileStorage, LocalFileStorage>();
            services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));

            // HTTP Context Accessor
            services.AddHttpContextAccessor();

            // Controllers with Validation
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            // API Versioning - Geçici olarak yorum satırı
            //services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ReportApiVersions = true;
            //});

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "CRM Banking API",
                    Version = "v1",
                    Description = "Banka ekstresi yükleme ve işleme API'si"
                });

                c.AddSecurityDefinition("Bearer", new()
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new()
                {
                    {
                        new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // Health Checks
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!,
                    name: "BankingDb",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "database", "sqlserver" });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("BankingApiPolicy", policy =>
                {
                    policy.WithOrigins(
                            configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                            ?? new[] { "http://localhost:3000" })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}