using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Domain.Common.Values.Guest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Context.EntityConfigurations
{
    public class VeaEventConfig : IEntityTypeConfiguration<VeaEvent>
    {
        public void Configure(EntityTypeBuilder<VeaEvent> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => EventId.From(value)
                )
                .ValueGeneratedNever();

            builder
                .Property(e => e.title)
                .HasColumnName("Title")
                .HasConversion(
                    vo => vo == null ? null : vo.Value,
                    s => s == null ? null : EventTitle.Create(s).Value
                    )
                .IsRequired();

            builder
                .Property(e => e.Description)
                .HasColumnName("Description")
                .HasConversion(
                    vo => vo == null ? null : vo.Value,
                    s => s == null ? null : EventDescription.Create(s).Value
                )
                .IsRequired(false);

            builder
                .OwnsOne(
                    e => e.EventDateTime,
                    dt =>
                    {
                        // Example: store the underlying DateTime properties in separate columns
                        dt.Property(d => d.StartDateTime).HasColumnName("StartDateTime");
                        dt.Property(d => d.EndDateTime).HasColumnName("EndDateTime");
                    });

            builder
                .Property(e => e.Visibility)
                .HasColumnName("Visibility")
                .HasConversion(
                    vo => vo.Value,                          // e.g. underlying enum/string
                    v => v == null ? null : EventVisibility.From(v)             // factory from primitive → VO
                )
                .IsRequired(false);

            builder
                .Property(e => e.status)
                .HasColumnName("Status")
                .HasConversion(
                    vo => vo.Value,                                 // e.g. underlying enum or int
                    v => EventStatus.From(v)                       // factory from primitive → VO
                )
                .IsRequired();


            builder.OwnsOne(
                e => e.Participants,
                ep =>
                {
                    // Map MaxGuests (NumberOfGuests → int)
                    ep.Property(p => p.MaxGuests)
                      .HasColumnName("MaxGuests")
                      .HasConversion(
                          vo => vo.Value,
                          i => NumberOfGuests.FromInt(i).Value
                      )
                      .IsRequired();

                    // Map private HashSet<GuestId> "_guests" into EventGuests
                    ep.OwnsMany<GuestId>(
                        "_guests",
                        gb =>
                        {
                            gb.ToTable("EventGuests");

                            // FK back to VeaEvent (shadow property EventId)
                            gb.WithOwner()
                              .HasForeignKey("EventId");

                            // The CLR property is .Value (Guid), stored in a column named "GuestId"
                            gb.Property(g => g.Value)
                              .HasColumnName("GuestId");

                            // PK uses (EventId, Value), which in SQL become columns (EventId, GuestId)
                            gb.HasKey("EventId", nameof(GuestId.Value));
                        }
                    );
                }
            );

            builder.ToTable("VeaEvents");
        }
    }
}
