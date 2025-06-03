namespace EventAssociation.Presentation.WebAPI.EndPoints.Event;

public class UpdateEventDescriptionRequest
{
    public string EventId { get; set; } = string.Empty;
    public string NewDescription { get; set; } = string.Empty;
}