namespace QueryContracts.DTOs
{
    public record UpcomingEventsDto
    (
        List<EventDetailsDto> UpcomingEvents,
        int MaxPageNumer
    );
}
