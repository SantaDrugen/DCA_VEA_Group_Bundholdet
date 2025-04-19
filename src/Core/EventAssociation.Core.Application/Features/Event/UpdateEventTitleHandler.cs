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

            Results getResult = await eventRepo.GetByIdAsync(command.id.Value);
            if (getResult.IsFailure)
                errors.AddRange(getResult.Errors);

            Results updateResult = await eventRepo.UpdateEventTitle(command.id.Value, command.newTtitle.Value);
            if (updateResult.IsFailure)
                errors.AddRange(updateResult.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await uow.SaveChangesAsync();

            return Results.Success();
        }
    }
}
