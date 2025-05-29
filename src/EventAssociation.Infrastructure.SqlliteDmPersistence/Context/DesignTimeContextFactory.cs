using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Context;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<VeaDbContext>
{
    public VeaDbContext CreateDbContext(String[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VeaDbContext>();
        optionsBuilder.UseSqlite(@"Data Source = VEADatabaseProduction.db");
        return new VeaDbContext(optionsBuilder.Options);
    }
}