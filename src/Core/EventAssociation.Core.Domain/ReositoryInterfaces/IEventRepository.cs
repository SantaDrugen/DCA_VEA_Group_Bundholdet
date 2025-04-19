using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.ReositoryInterfaces
{
    public interface IEventRepository
    {
        Task<Results<VeaEvent>> CreateAsync(VeaEvent @event);

        Task<Results<VeaEvent>> GetByIdAsync(EventId id);
        Task<Results> SetEventPrivate(EventId id);
        Task<Results> SetEventPublic(EventId id);
        Task<Results> SetEventStatusActive(EventId id);
        Task<Results> SetEventStatusReady(EventId id);
        Task<Results> UpdateEventDateTime(EventId id, EventDateTime dateTime);
        Task<Results> UpdateEventDescription(EventId id, EventDescription newDescription);
        Task<Results<NumberOfGuests>> UpdateEventMaxNumberOfGuests(EventId id, NumberOfGuests maxNumberOfGuests);
        Task<Results> UpdateEventTitle(EventId id, EventTitle newTitle);
    }
}
