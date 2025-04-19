using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.ReositoryInterfaces
{
    public interface IEventRepository
    {
        Task<Results<VeaEvent>> CreateAsync(VeaEvent @event);

        Task<Results<VeaEvent>> GetByIdAsync(Guid id);

        Task<Results> UpdateEventTitle(Guid id, string newTitle);
    }
}
