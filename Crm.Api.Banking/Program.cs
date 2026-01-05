using Crm.Data;
using Microsoft.EntityFrameworkCore;
// using Crm.Api.Shared.Platform;  // sende namespace neyse

var builder = WebApplication.CreateBuilder(args);

// 1) Ortak platform (Exception, Swagger policy, Validation, Health, CORS vs.)
// builder.Services.AddCrmApiPlatform(builder.Configuration);

// 2) Controllers
builder.Services.AddControllers();

// 3) DbContext (mevcut davranýþý koru)
builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"),
        sql => sql.EnableRetryOnFailure()); // transient hatalarda dayanýklýlýk
});

var app = builder.Build();

// 4) Ortak middleware pipeline
// app.UseCrmApiPlatform();

app.MapControllers();
app.Run();
