namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Models;

public partial class EventGuest
{
    public string EventId { get; set; } = null!;

    public string GuestId { get; set; } = null!;

    public virtual VeaEvent? Event { get; set; } = null!;
}
