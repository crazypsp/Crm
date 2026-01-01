using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crm.Data;

public sealed class CrmDbContextFactory : IDesignTimeDbContextFactory<CrmDbContext>
{
    public CrmDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CrmDbContext>()
            // Burayı kendi lokal SQL Server'ına göre güncelle
            .UseSqlServer("Server=DESKTOP-54QF28R\\ZRV2014EXP;Database=CrmSuiteDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
            .Options;

        return new CrmDbContext(options);
    }
}
