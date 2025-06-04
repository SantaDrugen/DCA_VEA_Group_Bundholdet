using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.Queries
{
    public record GetUpcomingEventsQuery
    (
        int PageNumber = 1,
        int PageSize = 1,
        string? SearchTerm = null
    ) :IQuery<UpcomingEventsDto>;
}
