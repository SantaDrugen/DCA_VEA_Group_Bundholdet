namespace EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;

public class ParticipateInPublicEventRequest
{
    public string EventId { get; set; } = string.Empty;
    public string GuestId { get; set; } = string.Empty;
}