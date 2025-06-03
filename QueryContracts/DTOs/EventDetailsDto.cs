namespace QueryContracts.DTOs
{
    public record EventDetailsDto(
        string EventId,
        string Title,
        string Description,
        string? LocationName,
        string? StartDateTime,
        string? EndDateTime,
        bool IsPublic,
        int CurrentAttendeeCount,
        int? MaxGuests,
        IReadOnlyList<GuestDto> Guests
    );
}
