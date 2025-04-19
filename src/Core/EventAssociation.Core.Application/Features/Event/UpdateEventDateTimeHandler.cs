using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class UpdateEventDateTimeHandler(IEventRepository repo, IUnitOfWork work) : ICommandHandler<UpdateEventDateTimeCommand>
    {
        private readonly IEventRepository eventRepo = repo;
        private readonly IUnitOfWork uow = work;
        public async Task<Results> HandleAsync(UpdateEventDateTimeCommand command)
        {
            List<Error> errors = new List<Error>();

            Results<VeaEvent> getResult = await eventRepo.GetByIdAsync(command.id);

            if (getResult.IsFailure)
                errors.AddRange(getResult.Errors);

            Results updateResult = await eventRepo.UpdateEventDateTime(command.id, command.newDateTime);

            if (updateResult.IsFailure)
                errors.AddRange(updateResult.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await eventRepo.UpdateEventDateTime(command.id, command.newDateTime);
            await uow.SaveChangesAsync();

            return Results.Success();
        }
    }
}
