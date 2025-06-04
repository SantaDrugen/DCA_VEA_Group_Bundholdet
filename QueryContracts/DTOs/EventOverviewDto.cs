namespace QueryContracts.DTOs
{
    public record EventOverviewDto
    (
        string EventId,
        string Title,
        int CurrentAttendeeCount,
        string? Date,
        string? StartTime
    );
}
