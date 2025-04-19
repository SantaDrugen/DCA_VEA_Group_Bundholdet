using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class SetEventStatusReadyHandler : ICommandHandler<SetEventStatusReadyCommand>
    {
        private readonly IEventRepository eventRepo;
        private readonly IUnitOfWork uow;

        public SetEventStatusReadyHandler(IEventRepository eventRepo, IUnitOfWork uow)
        {
            this.eventRepo = eventRepo;
            this.uow = uow;
        }

        public async Task<Results> HandleAsync(SetEventStatusReadyCommand command)
        {
            Results<VeaEvent> eventResult = await eventRepo.GetByIdAsync(command.id);

            if (eventResult.IsFailure)
                return eventResult;

            Results setEventStatusResult = await eventRepo.SetEventStatusReady(command.id);

            if (setEventStatusResult.IsFailure)
                return setEventStatusResult;

            Results saveResult = await uow.SaveChangesAsync();

            return Results.Success();
        }
    }
}
