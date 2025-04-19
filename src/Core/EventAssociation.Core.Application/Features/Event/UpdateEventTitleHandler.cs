using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class UpdateEventTitleHandler(IEventRepository repo, IUnitOfWork work) : ICommandHandler<UpdateEventTitleCommand>
    {
        private readonly IEventRepository eventRepo = repo;
        private readonly IUnitOfWork uow = work;

        public async Task<Results> HandleAsync(UpdateEventTitleCommand command)
        {
            List<Error> errors = new List<Error>();

            Results getResult = await eventRepo.GetByIdAsync(command.id);

            if (getResult.IsFailure)
                return Results.Failure(getResult.Errors.ToArray());

            Results updateResult = await eventRepo.UpdateEventTitle(command.id, command.newTtitle);
            if (updateResult.IsFailure)
                errors.AddRange(updateResult.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await uow.SaveChangesAsync();

            return Results.Success();
        }
    }
}
