namespace EventAssociation.Presentation.WebAPI.EndPoints.Event;

public class SetEventStatusRequest
{
    public string EventId { get; set; } = string.Empty;
    
    // TODO: Lookup event statusses from a predefined list or enum?
    public string NewStatus { get; set; } = string.Empty;
}