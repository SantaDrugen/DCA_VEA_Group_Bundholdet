namespace QueryContracts.DTOs
{
    public record GuestProfileDto(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string? PictureUrl,
        int UpcomingEventsCount,
        List<EventOverviewDto> UpcomingEvents,
        List<PastEventDto> PastEvents,
        int PendingInvitationCount
        );
}
