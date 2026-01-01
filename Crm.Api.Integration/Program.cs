using Crm.Data;
using Crm.Services.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Entegrasyon yönetimi CRUD + test endpoint’leri için en hýzlý yöntem.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Profile/Secret kayýtlarý DB’de duracak, bu servis DB’ye okuyup yazacak.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

// DataProtection: secret’larý protect/unprotect etmek için
var keyRingPath = builder.Configuration["Security:KeyRingPath"] ?? "App_Data/keys";
var appName = builder.Configuration["Security:ApplicationName"] ?? "CrmSuite";

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyRingPath))
    .SetApplicationName(appName);

// Secret protector (ortak servis)
builder.Services.AddSingleton<ISecretProtector, DataProtectionSecretProtector>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
