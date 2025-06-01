using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Repositories
{
    public class EventRepository
        : RepositoryBase<VeaEvent, EventId>, IEventRepository
    {
        public EventRepository(VeaDbContext context)
            : base(context) { }
    }
}
