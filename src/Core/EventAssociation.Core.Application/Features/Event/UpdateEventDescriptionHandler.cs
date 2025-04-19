using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class UpdateEventDescriptionHandler : ICommandHandler<UpdateEventDescriptionCommand>
    {
        private readonly IEventRepository eventRepo;
        private readonly IUnitOfWork uow;

        public UpdateEventDescriptionHandler(IEventRepository eventRepo, IUnitOfWork uow)
        {
            this.eventRepo = eventRepo;
            this.uow = uow;
        }

        public async Task<Results> HandleAsync(UpdateEventDescriptionCommand command)
        {
            List<Error> errors = new List<Error>();

            Results getResult = await eventRepo.GetByIdAsync(command.id);

            if (getResult.IsFailure)
                return Results.Failure(getResult.Errors.ToArray());

            Results updateResult = await eventRepo.UpdateEventDescription(command.id, command.newDescription);

            if (updateResult.IsFailure)
                errors.AddRange(updateResult.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await uow.SaveChangesAsync();

            return Results.Success();
        }
    }
}
