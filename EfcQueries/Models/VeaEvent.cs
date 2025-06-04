namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Models;

public partial class VeaEvent
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Status { get; set; }

    public int? Visibility { get; set; }

    public string? StartDateTime { get; set; }

    public string? EndDateTime { get; set; }

    public int? MaxGuests { get; set; }

    public virtual ICollection<EventGuest>? EventGuests { get; set; } = new List<EventGuest>();
}
