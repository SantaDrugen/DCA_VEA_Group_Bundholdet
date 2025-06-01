using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;

namespace EventAssociation.Core.Domain.ReositoryInterfaces
{
    public interface IEventRepository : IGenericRepository<VeaEvent, EventId>
    {

    }
}
