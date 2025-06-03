using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Models;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data source=C:\\Users\\emil0\\source\\repos\\DCA_VEA_Group_Bundholdet\\src\\EventAssociation.Infrastructure.SqlliteDmPersistence\\VEADatabaseProduction.db");

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
