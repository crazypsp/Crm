using Crm.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Bu API saf CRUD + query servisidir; controllers MVP için hýzlý ve yeterlidir.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: BankTemplate/Rule/Import/Transaction verilerini okuyup yazacaðýz.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
