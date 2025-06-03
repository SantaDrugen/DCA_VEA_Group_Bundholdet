namespace EventAssociation.Presentation.WebAPI.EndPoints.Event;

public class UpdateEventTitleRequest
{
    public string EventId { get; set; } = string.Empty;
    public string NewTitle { get; set; } = string.Empty;
}