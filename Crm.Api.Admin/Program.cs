using Crm.Data;
using Crm.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Neden: Admin CRUD operasyonlarý (Dealer/Tenant/Company/User) klasik REST ile en hýzlý yönetilir.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(opt =>
{
    // Neden: Identity + Tenancy tablolarý ayný context içinde.
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CrmDb"));
});

// Identity (User/Role yönetimi)
// Neden: Admin API; kullanýcý oluþturma, role atama, parola setleme gibi iþlemleri yapacak.
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        // Neden: MVP’de makul güvenlik politikalarý.
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<CrmDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Not: Authentication/Authorization (JWT vs) Gateway’de de olabilir.
// Burada MVP için açýk býrakýyoruz; prod’da [Authorize] + JWT ekleyeceðiz.
app.MapControllers();
app.Run();
