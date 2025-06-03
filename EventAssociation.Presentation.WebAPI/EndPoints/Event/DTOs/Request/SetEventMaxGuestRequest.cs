namespace EventAssociation.Presentation.WebAPI.Endpoints.Event;

public class SetEventMaxGuestsRequest
{
    public string EventId { get; set; } = string.Empty;
    public int MaxGuests { get; set; }
}