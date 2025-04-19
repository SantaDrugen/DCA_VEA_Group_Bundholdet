using System.Collections.Concurrent;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Tests.Infrastructure.Repositories
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly ConcurrentDictionary<EventId, VeaEvent> _store
            = new ConcurrentDictionary<EventId, VeaEvent>();

        public Task<Results<VeaEvent>> CreateAsync(VeaEvent @event)
        {
            if (!_store.TryAdd(@event.Id, @event))
                return Task.FromResult(Results<VeaEvent>.Failure(new Error("ID_ALREADY_EXISTS", "An event with the same Id already exists.")));

            return Task.FromResult(Results<VeaEvent>.Success(@event));
        }

        public Task<Results<VeaEvent>> GetByIdAsync(EventId id)
        {
            if (_store.TryGetValue(id, out var existing))
                return Task.FromResult(Results<VeaEvent>.Success(existing));

            return Task.FromResult(Results<VeaEvent>.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }

        public Task<Results> SetEventPublic(EventId id)
        {
            if (_store.TryGetValue(id, out var existing))
            {
                existing.SetVisibilityPublic();
                return Task.FromResult(Results.Success());
            }
            return Task.FromResult(Results.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }

        public Task<Results> UpdateEventDateTime(EventId id, EventDateTime dateTime)
        {
            if (_store.TryGetValue(id, out var existing))
            {
                existing.SetDateTime(dateTime);
                return Task.FromResult(Results.Success());
            }
            return Task.FromResult(Results.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }

        public Task<Results> UpdateEventDescription(EventId id, EventDescription newDescription)
        {
            if (_store.TryGetValue(id, out var existing))
            {
                existing.SetDescription(newDescription.Value);
                return Task.FromResult(Results.Success());
            }
            return Task.FromResult(Results.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }

        public Task<Results> UpdateEventTitle(EventId id, EventTitle newTitle)
        {
            if (_store.TryGetValue(id, out var existing))
            {
                existing.SetTitle(newTitle.Value);
                return Task.FromResult(Results.Success());
            }
            return Task.FromResult(Results.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }
    }
}
