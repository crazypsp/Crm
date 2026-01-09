using Crm.Api.Admin.Extensions;
using Crm.Api.Admin.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new Serilog.LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/admin-api-.txt", rollingInterval: Serilog.RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("CRM Admin API starting up...");

    // Add services to the container
    builder.Services.AddAdminApiServices(builder.Configuration);

    // Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.RequireHttpsMetadata = builder.Configuration.GetValue<bool>("Authentication:RequireHttpsMetadata", false);

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"]
        };
    });

    // Authorization with Policy
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("Admin", "SuperAdmin"));

        options.AddPolicy("SuperAdminOnly", policy =>
            policy.RequireRole("SuperAdmin"));
    });

    // Add Serilog
    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Admin API v1");
            c.RoutePrefix = "api-docs";
        });

        app.UseDeveloperExceptionPage();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("AdminApiPolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    // Global Exception Handling Middleware
    app.UseMiddleware<GlobalExceptionMiddleware>();

    app.MapControllers();

    // Health check endpoint
    app.MapGet("/health", () => Results.Ok(new
    {
        status = "Healthy",
        service = "Admin API",
        timestamp = DateTime.UtcNow
    }));

    Log.Information("CRM Admin API started successfully on port 5006");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "CRM Admin API failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}