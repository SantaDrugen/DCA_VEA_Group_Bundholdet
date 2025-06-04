using System;
using System.Collections.Generic;
using System.Globalization;
using EfcQueries.Queries;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.QueryHandlers
{
    public class GetUpcomingEventsQueryHandler : IQueryHandler<GetUpcomingEventsQuery, UpcomingEventsDto>
    {
        private readonly VeadatabaseProductionContext _readContext;

        public GetUpcomingEventsQueryHandler(VeadatabaseProductionContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<UpcomingEventsDto> HandleAsync(GetUpcomingEventsQuery query)
        {
            var nowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            var baseQuery = _readContext.VeaEvents
                .Where(e => e.StartDateTime != null && e.StartDateTime.CompareTo(nowString) >= 0);

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                baseQuery = baseQuery.Where(e => e.Title.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = await baseQuery.CountAsync();
            var maxPageNumber = (int)Math.Ceiling((double)totalCount / query.PageSize);

            var eventsList = await baseQuery
                .OrderBy(e => e.StartDateTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(e => new EventDetailsDto(
                    e.Id,
                    e.Title,
                    e.Description.Substring(0, int.Min(e.Description.Length, 100)),
                    null,
                    e.StartDateTime,
                    null,
                    e.Visibility == 1,
                    e.EventGuests.Count,
                    e.MaxGuests,
                    null
                ))
                .ToListAsync();

            return new UpcomingEventsDto(eventsList, maxPageNumber);
        }
    }
}
