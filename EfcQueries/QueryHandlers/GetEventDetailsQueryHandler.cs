using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.DTOs;
using QueryContracts.Queries;

namespace EfcQueries.QueryHandlers
{
    public class GetEventDetailsQueryHandler : IQueryHandler<GetEventDetailsQuery, EventDetailsDto>
    {
        private readonly VeadatabaseProductionContext _readContext; // Using the generated Read-context - 
                                                                    // This context reads the entities as simple types for easy handover to DTOs.
        public GetEventDetailsQueryHandler(VeadatabaseProductionContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<EventDetailsDto> HandleAsync(GetEventDetailsQuery query)
        {
            var eventEntity = await _readContext.VeaEvents
                .SingleOrDefaultAsync(e => e.Id == query.EventId) 
                ?? 
                throw new InvalidOperationException($"Event with ID {query.EventId} not found.");

            bool isPublic = eventEntity.Visibility.HasValue && eventEntity.Visibility.Value == 1;

            var guestQuery = from eg in _readContext.EventGuests
                             join g in _readContext.VeaGuests on eg.GuestId equals g.Id
                             where eg.EventId == eventEntity.Id
                             select g;

            int currentAttendeeCount = await guestQuery.CountAsync();
            int? maxGuests = eventEntity.MaxGuests;

            var Skip = query.Skip < 0 ? 0 : query.Skip;
            var Take = query.Take <= 0 ? 10 : query.Take; // Default to 10 if Take is not specified or invalid

            if (query.Skip + query.Take > currentAttendeeCount)
            {
                Take = Math.Max(0, currentAttendeeCount - Skip);
            }

            var pagedGuests = await guestQuery
                .OrderBy(g => g.FirstName)
                .Skip(Skip)
                .Take(Take)
                .Select(g => new GuestDto(
                    g.Id,
                    g.FirstName + " " + g.LastName,
                    g.PictureUrl,
                    g.Email
                ))
                .ToListAsync();

            return new EventDetailsDto(
                EventId:                eventEntity.Id,
                Title:                  eventEntity.Title,
                Description:            eventEntity.Description ?? "",
                LocationName:           null, // Not implemented
                StartDateTime:          eventEntity.StartDateTime,
                EndDateTime:            eventEntity.EndDateTime,
                IsPublic:               isPublic,
                CurrentAttendeeCount:   currentAttendeeCount,
                MaxGuests:              maxGuests,
                Guests:                 pagedGuests
                );
        }
    }
}
