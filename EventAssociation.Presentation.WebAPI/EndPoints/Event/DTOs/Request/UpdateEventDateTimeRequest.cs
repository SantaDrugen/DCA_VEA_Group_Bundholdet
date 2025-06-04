namespace EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;

public class UpdateEventDateTimeRequest
{
    public string EventId { get; set; } = string.Empty;
    public DateTimeOffset NewEventDateTime { get; set; }
}