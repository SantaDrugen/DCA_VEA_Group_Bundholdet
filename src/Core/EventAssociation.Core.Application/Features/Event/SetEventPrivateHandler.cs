using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Event
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

            Results<VeaEvent> result = await eventRepo.GetByIdAsync(command.id);

            if (result.IsFailure)
                return Results.Failure(result.Errors.ToArray());

            Results repoResult = await eventRepo.SetEventPrivate(command.id);

            if (repoResult.IsFailure)
                errors.AddRange(repoResult.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await uow.SaveChangesAsync();

            return Results.Success();
        }
    }
}
