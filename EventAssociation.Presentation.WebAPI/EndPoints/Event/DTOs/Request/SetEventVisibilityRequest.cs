namespace EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;

public class SetEventVisibilityRequest
{
    public string EventId { get; set; } = string.Empty;
    // True to make the event private, false to make it public
    public bool MakePrivate { get; set; }
}