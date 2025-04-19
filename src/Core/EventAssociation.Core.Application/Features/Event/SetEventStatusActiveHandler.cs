using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class SetEventStatusActiveHandler : ICommandHandler<SetEventStatusActiveCommand>
    {
        private readonly IEventRepository eventRepo;
        private readonly IUnitOfWork uow;

        public SetEventStatusActiveHandler(IEventRepository eventRepo, IUnitOfWork uow)
        {
            this.eventRepo = eventRepo;
            this.uow = uow;
        }

        public async Task<Results> HandleAsync(SetEventStatusActiveCommand command)
        {
            Results<VeaEvent> eventResult = await eventRepo.GetByIdAsync(command.id);

            if (eventResult.IsFailure)
                return eventResult;

            Results result = await eventRepo.SetEventStatusActive(command.id);
            if (result.IsFailure)
                return result;

            await uow.SaveChangesAsync();

            return result;
        }
    }
}
