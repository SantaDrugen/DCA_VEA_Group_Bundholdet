using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Application.Handlers;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Handlers.Event
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
            List<Error> errors = new List<Error>();

            Results<VeaEvent> eventResult = await eventRepo.GetAsync(command.id);

            if (eventResult.IsFailure)
                errors.AddRange(eventResult.Errors);

            var eventEntity = eventResult.Value;

            if (eventEntity is not null)
            {
                var updateResult = eventEntity.SetActive();

                if (updateResult.IsFailure)
                    errors.AddRange(updateResult.Errors);

                var saveResult = await uow.SaveChangesAsync();

                if (saveResult.IsFailure)
                    errors.AddRange(saveResult.Errors);
            }

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            var updatedEventResult = Results<VeaEvent>.Success(eventEntity);

            return updatedEventResult;
        }
    }
}
