using Crm.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Work modülü CRUD + atama gibi standart HTTP iþlemleri içerir; controller yaklaþýmý MVP’de en hýzlý yoldur.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Görev ve atama tablolarýný okuyup yazacaðýz.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
