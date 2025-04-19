using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class SetEventMaxNumberOfGuestsHandler : ICommandHandler<SetEventMaxNumberOfGuestsCommand>
    {
        private readonly IEventRepository eventRepo;
        private readonly IUnitOfWork uow;

        public SetEventMaxNumberOfGuestsHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            eventRepo = eventRepository;
            uow = unitOfWork;
        }

        public async Task<Results> HandleAsync(SetEventMaxNumberOfGuestsCommand command)
        {
            Results<VeaEvent> eventResult = await eventRepo.GetByIdAsync(command.id);

            if (eventResult.IsFailure)
                return eventResult;

            Results<NumberOfGuests> guestsResult = await eventRepo.UpdateEventMaxNumberOfGuests(command.id, command.MaxNumberOfGuests);

            if (guestsResult.IsFailure)
                return guestsResult;

            Results saveResult = await uow.SaveChangesAsync();

            if (saveResult.IsFailure)
                return saveResult;

            return Results.Success();
        }
    }
}
