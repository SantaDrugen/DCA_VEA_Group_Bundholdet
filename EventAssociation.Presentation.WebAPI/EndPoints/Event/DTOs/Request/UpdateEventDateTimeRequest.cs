namespace EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;

public class UpdateEventDateTimeRequest
{
    public string EventId { get; set; }
    public DateTime NewEventDateTimeStart { get; set; }
    public DateTime NewEventDateTimeEnd { get; set; }
}