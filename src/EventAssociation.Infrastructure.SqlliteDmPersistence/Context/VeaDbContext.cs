using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Guest;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Context;

public class VeaDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeaDbContext).Assembly);
    }

    public DbSet<VeaEvent> Events => Set<VeaEvent>();

    public DbSet<VeaGuest> Guests => Set<VeaGuest>();
}