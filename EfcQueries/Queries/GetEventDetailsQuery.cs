using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.Queries
{
    public record GetEventDetailsQuery
    (
        string EventId,
        int Skip, // number of items to skip for pagination
        int Take // number of items to take for pagination
    ) : IQuery<EventDetailsDto>;
}
