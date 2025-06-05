using EfcQueries.Models;
using EfcQueries.Queries;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.QueryHandlers
{
    public class GetUnpublishedEventsQueryHandler : IQueryHandler<GetUnpublishedEventsQuery, UnpublishedEventsDto>
    {
        private readonly VeadatabaseProductionContext _readContext;

        public GetUnpublishedEventsQueryHandler(VeadatabaseProductionContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<UnpublishedEventsDto> HandleAsync(GetUnpublishedEventsQuery query)
        {
            // Drafts (Status == 0)
            var draftQuery = _readContext.VeaEvents
                .Where(e => e.Status == 0);
            var draftCount = await draftQuery.CountAsync();
            int draftMaxPageNumber = (int)Math.Ceiling((double)draftCount / query.PageSize);

            var draftEvents = await draftQuery
                .OrderByDescending(e => e.StartDateTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(e => new SimpleEventDetails(e.Id, e.Title))
                .ToListAsync();

            // Ready (Status == 2)
            var readyQuery = _readContext.VeaEvents
                .Where(e => e.Status == 2);
            var readyCount = await readyQuery.CountAsync();
            int readyMaxPageNumber = (int)Math.Ceiling((double)readyCount / query.PageSize);

            var readyEvents = await readyQuery
                .OrderByDescending(e => e.StartDateTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(e => new SimpleEventDetails(e.Id, e.Title))
                .ToListAsync();

            // Cancelled (Status == 4)
            var cancelledQuery = _readContext.VeaEvents
                .Where(e => e.Status == 4);
            var cancelledCount = await cancelledQuery.CountAsync();
            int cancelledMaxPageNumber = (int)Math.Ceiling((double)cancelledCount / query.PageSize);

            var cancelledEvents = await cancelledQuery
                .OrderByDescending(e => e.StartDateTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(e => new SimpleEventDetails(e.Id, e.Title))
                .ToListAsync();

            return new UnpublishedEventsDto(
                draftEvents,
                readyEvents,
                cancelledEvents,
                draftMaxPageNumber,
                readyMaxPageNumber,
                cancelledMaxPageNumber
            );
        }
    }
}
