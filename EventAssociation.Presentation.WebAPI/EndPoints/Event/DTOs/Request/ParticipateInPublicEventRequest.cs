namespace EventAssociation.Presentation.WebAPI.EndPoints.Event;

public class ParticipateInPublicEventRequest
{
    public string EventId { get; set; } = string.Empty;
    public string GuestId { get; set; } = string.Empty;
}