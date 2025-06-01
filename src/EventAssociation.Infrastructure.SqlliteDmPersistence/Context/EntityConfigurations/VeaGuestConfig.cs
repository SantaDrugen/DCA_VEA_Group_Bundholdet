using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common.Values.Guest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Context.EntityConfigurations
{
    public class VeaGuestConfig : IEntityTypeConfiguration<VeaGuest>
    {
        public void Configure(EntityTypeBuilder<VeaGuest> builder)
        {
            // 1) Primary key: GuestId <-> Guid
            builder.HasKey(g => g.Id);
            builder
                .Property(g => g.Id)
                .HasConversion(
                    id => id.Value,            // GuestId → Guid
                    raw => GuestId.From(raw)     // Guid → GuestId
                )
                .ValueGeneratedNever();

            // 2) Map EmailAddress (string underneath)
            builder
                .Property(g => g.Email)
                .HasColumnName("Email")
                .HasConversion(
                    vo => vo.Value,                              // EmailAddress → string
                    s => EmailAddress.From(s).Value            // string → EmailAddress
                )
                .IsRequired();

            // 3) Map FirstName (string underneath)
            builder
                .Property(g => g.FirstName)
                .HasColumnName("FirstName")
                .HasConversion(
                    vo => vo.Value,                              // FirstName → string
                    s => FirstName.From(s).Value              // string → FirstName
                )
                .IsRequired();

            // 4) Map LastName (string underneath)
            builder
                .Property(g => g.LastName)
                .HasColumnName("LastName")
                .HasConversion(
                    vo => vo.Value,                              // LastName → string
                    s => LastName.From(s).Value                // string → LastName
                )
                .IsRequired();

            // 5) Map PictureUrl (string underneath)
            builder
                .Property(g => g.PictureUrl)
                .HasColumnName("PictureUrl")
                .HasConversion(
                    vo => vo.Value,                              // PictureUrl → string/URL
                    s => PictureUrl.From(s)              // string → PictureUrl
                )
                .IsRequired(false);  // or .IsRequired() if you never allow null

            // 6) (If you want to rename the table)
            builder.ToTable("VeaGuests");
        }
    }
}
