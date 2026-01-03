using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crm.Data;

public sealed class CrmDbContextFactory : IDesignTimeDbContextFactory<CrmDbContext>
{
    public CrmDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CrmDbContext>();

        // Neden: Design-time migration sırasında startup bağımlılığını azaltmak.
        // Burayı kendi local connection string'in ile güncelle.
        options.UseSqlServer(
            "Server=DESKTOP-54QF28R\\ZRV2014EXP;Database=CrmSuiteDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
        );

        return new CrmDbContext(options.Options);
    }
}