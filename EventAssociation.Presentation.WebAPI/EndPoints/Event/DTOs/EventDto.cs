namespace EventAssociation.Presentation.WebAPI.EndPoints.Event;

public class EventDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset EventDateTime { get; set; }
    public string Visibility { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int MaxGuests { get; set; }
    public int CurrentParticipants { get; set; }
}