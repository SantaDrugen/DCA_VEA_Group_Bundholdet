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

            int currentAttendeeCount = 0; //Not implemented
            int? maxGuests = eventEntity.MaxGuests;

            var pagedGuests = new List<GuestDto>(); //Not implemented

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
