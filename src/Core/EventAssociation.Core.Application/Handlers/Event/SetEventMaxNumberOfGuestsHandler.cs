using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Application.Handlers;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Handlers.Event
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
            List<Error> errors = new List<Error>();

            Results<VeaEvent> eventResult = await eventRepo.GetAsync(command.id);

            if (eventResult.IsFailure)
                errors.AddRange(eventResult.Errors);

            VeaEvent eventEntity = eventResult.Value;

            if (eventEntity is not null)
            {
                var updateResult = eventEntity.SetMaxGuests(command.MaxNumberOfGuests.Value);

                if (updateResult.IsFailure)
                    errors.AddRange(updateResult.Errors);

                Results saveResult = await uow.SaveChangesAsync();

                if (saveResult.IsFailure)
                    errors.AddRange(saveResult.Errors);
            }

            if (errors.Any())
                return Results.Failure(errors);

            var updatedEventResult = Results<VeaEvent>.Success(eventEntity);

            return updatedEventResult;
        }
    }
}
