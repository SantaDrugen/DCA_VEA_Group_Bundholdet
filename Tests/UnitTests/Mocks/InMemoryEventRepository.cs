using System.Collections.Concurrent;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Mocks
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly ConcurrentDictionary<EventId, VeaEvent> _store
            = new ConcurrentDictionary<EventId, VeaEvent>();

        public Task<Results> AddAsync(VeaEvent aggregate)
        {
            if (aggregate is null)
                return Task.FromResult(Results.Failure(new Error("ARGUMENT_NULL", "Aggregate cannot be null.")));

            if (!_store.TryAdd(aggregate.Id, aggregate))
                return Task.FromResult(Results.Failure(
                    new Error("ID_ALREADY_EXISTS", $"An event with Id '{aggregate.Id.Value}' already exists.")
                ));

            return Task.FromResult(Results.Success());
        }

        public Task<Results<VeaEvent>> GetAsync(EventId id)
        {
            if (id is null)
                return Task.FromResult(Results<VeaEvent>.Failure(
                    new Error("ARGUMENT_NULL", "EventId cannot be null.")
                ));

            if (_store.TryGetValue(id, out var existing))
                return Task.FromResult(Results<VeaEvent>.Success(existing));

            return Task.FromResult(Results<VeaEvent>.Failure(
                new Error("ID_NOT_FOUND", $"Event with Id '{id.Value}' not found.")
            ));
        }

        public Task<Results> RemoveAsync(EventId id)
        {
            if (id is null)
                return Task.FromResult(Results.Failure(new Error("ARGUMENT_NULL", "EventId cannot be null.")));

            if (!_store.TryRemove(id, out _))
                return Task.FromResult(Results.Failure(
                    new Error("ID_NOT_FOUND", $"Event with Id '{id.Value}' not found.")
                ));

            return Task.FromResult(Results.Success());
        }
    }
}
