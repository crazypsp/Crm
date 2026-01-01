using Crm.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Mesajlaþma MVP’si için REST endpoint'ler yeterli. Sonraki fazda SignalR eklenebilir.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Thread/Participant/Message verilerini bu DB’den okuyup yazacaðýz.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
