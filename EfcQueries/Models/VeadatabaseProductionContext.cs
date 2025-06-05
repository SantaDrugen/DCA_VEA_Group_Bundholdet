using EventAssociation.Infrastructure.SqlliteDmPersistence;
using Microsoft.EntityFrameworkCore;

namespace EfcQueries.Models;

public partial class VeadatabaseProductionContext : DbContext
{
    public VeadatabaseProductionContext()
    {
    }

    public VeadatabaseProductionContext(DbContextOptions<VeadatabaseProductionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EfmigrationsLock> EfmigrationsLocks { get; set; }

    public virtual DbSet<EventGuest> EventGuests { get; set; }

    public virtual DbSet<VeaEvent> VeaEvents { get; set; }

    public virtual DbSet<VeaGuest> VeaGuests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VeaDatabaseProduction.db");
        optionsBuilder.UseSqlite($@"Data Source={dbPath};");
    }
        

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EfmigrationsLock>(entity =>
        {
            entity.ToTable("__EFMigrationsLock");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<EventGuest>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.GuestId });

            entity.HasOne(d => d.Event).WithMany(p => p.EventGuests).HasForeignKey(d => d.EventId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
