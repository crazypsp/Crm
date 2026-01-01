using Crm.Api.Gateway.Middleware;
using Crm.Business.Banking;
using Crm.Business.Documents;
using Crm.Business.Integration;
using Crm.Business.Messaging;
using Crm.Business.Tenancy;
using Crm.Business.Work;
using Crm.Data;
using Crm.Services.Banking;
using Crm.Services.Common;
using Crm.Services.Integration;
using Crm.Services.Security;
using Crm.Services.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB (Crm.Data)
builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

// Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// ===== Crm.Business DI =====
// Tenancy
builder.Services.AddScoped<ITenancyManager, TenancyManager>();

// Banking
builder.Services.AddScoped<IBankMappingEngine, BankMappingEngine>();
builder.Services.AddScoped<IVoucherDraftBuilder, VoucherDraftBuilder>();
builder.Services.AddScoped<IBankImportManager, BankImportManager>();

// Work / Documents / Messaging / Integration
builder.Services.AddScoped<IWorkTaskManager, WorkTaskManager>();
builder.Services.AddScoped<IDocumentRequestManager, DocumentRequestManager>();
builder.Services.AddScoped<IMessageManager, MessageManager>();
builder.Services.AddScoped<IIntegrationJobManager, IntegrationJobManager>();

// ===== Crm.Services DI =====
builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("FileStorage"));
builder.Services.AddSingleton<IClock, SystemClock>();

builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

builder.Services.AddDataProtection();
builder.Services.AddSingleton<ISecretProtector, DataProtectionSecretProtector>();

// Banking services
builder.Services.AddScoped<ExcelStatementExtractor>();
builder.Services.AddScoped<PdfStatementExtractor>();
builder.Services.AddScoped<IBankStatementNormalizer, BankStatementNormalizer>();
builder.Services.AddScoped<IBankImportAppService, BankImportAppService>();

// Integration services
builder.Services.AddScoped<IIntegrationDispatcher, IntegrationDispatcher>();

var app = builder.Build();

// Pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
