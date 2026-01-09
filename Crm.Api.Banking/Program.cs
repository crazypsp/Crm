using Crm.Api.Banking.Extensions;
using Crm.Api.Banking.Middleware;
using Crm.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/banking-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddBankingApiServices(builder.Configuration);

// Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.RequireHttpsMetadata = builder.Configuration.GetValue<bool>("Authentication:RequireHttpsMetadata");

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"]
        };
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BankingUser", policy =>
        policy.RequireClaim("scope", "banking.api"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Banking API v1");
        c.RoutePrefix = "api-docs";
    });

    // Development only middleware
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors("BankingApiPolicy");
app.UseAuthentication();
app.UseAuthorization();

// Global Exception Handling Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Health Check Endpoint
app.MapHealthChecks("/health")
    .AllowAnonymous();

// API Controllers
app.MapControllers();

// Fallback for SPA (if needed)
app.MapFallbackToFile("index.html");

// Database Migration (Development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
    await dbContext.Database.MigrateAsync();
}

Log.Information("CRM Banking API starting on {Url}",
    builder.Configuration["ASPNETCORE_URLS"] ?? "https://localhost:5002");

await app.RunAsync();