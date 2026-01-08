using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crm.Data;

public sealed class CrmDbContextFactory : IDesignTimeDbContextFactory<CrmDbContext>
{
    public CrmDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CrmDbContext>();

        var cs = Environment.GetEnvironmentVariable("CRM_CONNECTION_STRING")
                ?? "Server=DESKTOP-54QF28R\\ZRV2014EXP;Database=CrmSuiteDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        options.UseSqlServer(cs, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("Crm.Data");
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });

        return new CrmDbContext(options.Options);
    }
}