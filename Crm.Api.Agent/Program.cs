using Crm.Api.Shared.Platform;
using Crm.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/agent-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Shared platform (Auth/ProblemDetails/Swagger/Health/RateLimit/CORS)
builder.Services.AddCrmApiPlatform(builder.Configuration);

// DbContext (Agent job tablosuna eriþecek)
builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

var app = builder.Build();

app.UseCrmApiPlatform();

app.Run();
