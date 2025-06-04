namespace EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;

public class SetEventMaxGuestsRequest
{
    public string EventId { get; set; } = string.Empty;
    public int MaxGuests { get; set; }
}