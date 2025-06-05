using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Handlers.Event
{
    public class SetEventPrivateHandler : ICommandHandler<SetEventPrivateCommand>
    {
        private readonly IEventRepository eventRepo;
        private readonly IUnitOfWork uow;

        public SetEventPrivateHandler(IEventRepository eventRepo, IUnitOfWork uow)
        {
            this.eventRepo = eventRepo;
            this.uow = uow;
        }

        public async Task<Results> HandleAsync(SetEventPrivateCommand command)
        {
            List<Error> errors = new List<Error>();

            Results<VeaEvent> result = await eventRepo.GetAsync(command.id);

            if (result.IsFailure)
                errors.AddRange(result.Errors);

            var eventEntity = result.Value;

            if (eventEntity is not null)
            {
                var updateResult = eventEntity.SetVisibilityPrivate();

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
