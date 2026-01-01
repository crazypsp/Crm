using Crm.Api.Agent.Security;
using Crm.Data;
using Crm.Services.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Agent ile iletiþim REST üzerinden olacak; controller tabaný MVP’de en hýzlý yaklaþým.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Agent da DB’ye yazacak (job claim/complete), dolayýsýyla CrmDbContext’e ihtiyaç var.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

// DataProtection
// Neden: ConnectionSecret gibi þifreli deðerleri ileride Agent tarafýnda çözmemiz gerekecek.
// Gateway ve Agent ayný key-ring ve app name kullanmazsa þifre çözme baþarýsýz olur.
var keyRingPath = builder.Configuration["Agent:KeyRingPath"] ?? "App_Data/keys";
var appName = builder.Configuration["Agent:ApplicationName"] ?? "CrmSuite";

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyRingPath))
    .SetApplicationName(appName);

// Secret protector
// Neden: Þifreli alanlar (ConnectionSecret) için ortak soyutlama.
builder.Services.AddSingleton<ISecretProtector, DataProtectionSecretProtector>();

// Agent auth middleware
// Neden: Agent endpoint’lerini header tabanlý key ile korumak (MVP için JWT’den daha basit).
builder.Services.Configure<AgentAuthOptions>(builder.Configuration.GetSection("Agent"));
builder.Services.AddScoped<AgentAuthMiddleware>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Agent auth middleware: /api/agent/** çaðrýlarýný korur.
// Register endpoint’i X-Registration-Key ile, diðerleri X-Agent-Key ile doðrulanýr.
app.UseMiddleware<AgentAuthMiddleware>();

app.MapControllers();
app.Run();
