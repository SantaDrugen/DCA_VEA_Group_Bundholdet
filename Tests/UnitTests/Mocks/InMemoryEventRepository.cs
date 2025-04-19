using System.Collections.Concurrent;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Tests.Infrastructure.Repositories
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly ConcurrentDictionary<Guid, VeaEvent> _store
            = new ConcurrentDictionary<Guid, VeaEvent>();

        public Task<Results<VeaEvent>> CreateAsync(VeaEvent @event)
        {
            if (!_store.TryAdd(@event.Id.Value, @event))
                return Task.FromResult(Results<VeaEvent>.Failure(new Error("ID_ALREADY_EXISTS", "An event with the same Id already exists.")));

            return Task.FromResult(Results<VeaEvent>.Success(@event));
        }

        public Task<Results<VeaEvent>> GetByIdAsync(Guid id)
        {
            if (_store.TryGetValue(id, out var existing))
                return Task.FromResult(Results<VeaEvent>.Success(existing));

            return Task.FromResult(Results<VeaEvent>.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }

        public Task<Results> UpdateEventTitle(Guid id, string newTitle)
        {
            if (_store.TryGetValue(id, out var existing))
            {
                existing.SetTitle(newTitle);
                return Task.FromResult(Results.Success());
            }
            return Task.FromResult(Results.Failure(new Error("ID_NOT_FOUND", $"Event with Id '{id}' not found.")));
        }
    }
}
