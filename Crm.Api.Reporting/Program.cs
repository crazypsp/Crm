using Crm.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Reporting sadece query endpoint’leri sunar; controller tabanlý yapý yeterli.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Raporlar DB’den okunur.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
