using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
{
    public class SetEventPublicHandler(IEventRepository repo, IUnitOfWork work) : ICommandHandler<SetEventPublicCommand>
    {
        private readonly IEventRepository eventRepo = repo;
        private readonly IUnitOfWork uow = work;
        public async Task<Results> HandleAsync(SetEventPublicCommand command)
        {
            List<Error> errors = new List<Error>();

            Results<VeaEvent> getResult = await eventRepo.GetByIdAsync(command.id);
            if (getResult.IsFailure)
                return Results.Failure(getResult.Errors.ToArray());

            Results result = await eventRepo.SetEventPublic(command.id);

            if (result.IsFailure)
                errors.AddRange(result.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await uow.SaveChangesAsync();

            return result;
        }
    }
}
