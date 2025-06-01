using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Context;

public class VeaDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeaDbContext).Assembly);

        modelBuilder.ApplyConfiguration(new VeaEventConfig());
        modelBuilder.ApplyConfiguration(new VeaEventConfig());
    }

    public DbSet<VeaEvent> Events => Set<VeaEvent>();

    public DbSet<VeaGuest> Guests => Set<VeaGuest>();
}