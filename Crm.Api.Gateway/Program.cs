using Crm.Api.Shared.Platform;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Neden: Üretim log standardý (structured log) + günlük rolling file.
// Loglar support/debug için kritik.
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/gateway-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Neden: Tüm platform standartlarýný tek noktadan kuruyoruz.
builder.Services.AddCrmApiPlatform(builder.Configuration);

var app = builder.Build();

// Neden: Middleware sýrasý standardize edilir (CorrelationId, ProblemDetails, Auth, Swagger, Health vs.)
app.UseCrmApiPlatform();

app.Run();
