using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class CreateNewEventHandler(IEventRepository repo, IUnitOfWork work) : ICommandHandler<CreateNewEventCommand>
    {
        private readonly IEventRepository eventRepo = repo;
        private readonly IUnitOfWork uow = work;

        public async Task<Results> HandleAsync(CreateNewEventCommand command)
        {
            // Implementation goes here
            Results result = VeaEvent.CreateNewEvent();
            if (result.IsSuccess)
            {
                await uow.SaveChangesAsync();
            }
            return result;
        }
    }
}
