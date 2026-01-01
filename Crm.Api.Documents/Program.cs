using Crm.Api.Documents.Storage;
using Crm.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Evrak modülü HTTP CRUD + Upload için controller tabanlý yapý MVP’de en hýzlý yoldur.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Evrak talepleri ve dosya metadatasý DB’de tutulur.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

// Neden: Disk/S3/MinIO gibi farklý storage backend’lerine geçiþi kolaylaþtýrmak için soyutlama.
builder.Services.AddSingleton<IFileStorage>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var root = cfg["Storage:RootPath"] ?? "App_Data/uploads";
    return new LocalFileStorage(root);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
