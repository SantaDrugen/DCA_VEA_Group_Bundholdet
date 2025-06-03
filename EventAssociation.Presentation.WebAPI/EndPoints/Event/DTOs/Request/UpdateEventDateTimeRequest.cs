namespace EventAssociation.Presentation.WebAPI.EndPoints.Event;

public class UpdateEventDateTimeRequest
{
    public string EventId { get; set; } = string.Empty;
    public DateTimeOffset NewEventDateTime { get; set; }
}