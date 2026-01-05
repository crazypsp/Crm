using System.Text;
using Crm.Api.Import.Parsing;
using Crm.Api.Import.Storage;
using Crm.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Neden: ExcelDataReader bazý excel’lerde code page encoding ister.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Neden: Büyük PDF/Excel yüklenebilir; multipart limit yükseltilir.
var maxBytes = builder.Configuration.GetValue<long>("Import:MaxUploadBytes", 100_000_000);
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = maxBytes;
});

// Neden: Import, DB’ye yazacaðý için DbContext gerekiyor.
builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

// Neden: Dosyalarý Documents standardýnda diske yazan storage servisimiz.
builder.Services.AddSingleton<IImportFileStorage>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var env = sp.GetRequiredService<IWebHostEnvironment>();

    // Neden: RootPath relative ise ContentRoot’a baðlayalým (stabil path).
    var root = cfg["Storage:RootPath"] ?? "App_Data/imports";
    var absoluteRoot = Path.IsPathRooted(root) ? root : Path.Combine(env.ContentRootPath, root);

    return new LocalImportFileStorage(absoluteRoot);
});

// Neden: Parser pipeline (Excel/PDF + orchestrator)
builder.Services.AddScoped<IBankStatementParser, ExcelBankStatementParser>();
builder.Services.AddSingleton<ITempFileStore, LocalTempFileStore>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
