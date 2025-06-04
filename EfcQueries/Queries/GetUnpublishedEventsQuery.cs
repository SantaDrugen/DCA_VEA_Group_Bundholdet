using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.Queries
{
    public record GetUnpublishedEventsQuery
    (
        int PageNumber = 1,
        int PageSize = 5
    ) : IQuery<UnpublishedEventsDto>;
}
