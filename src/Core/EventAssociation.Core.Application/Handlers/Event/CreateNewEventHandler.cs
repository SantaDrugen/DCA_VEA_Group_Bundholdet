using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Application.Handlers;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Handlers.Event
{
    public class CreateNewEventHandler : ICommandHandler<CreateNewEventCommand>
    {
        private readonly IEventRepository eventRepo;
        private readonly IUnitOfWork uow;

        public CreateNewEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            eventRepo = eventRepository;
            uow = unitOfWork;
        }

        public async Task<Results> HandleAsync(CreateNewEventCommand command)
        {
            List<Error> errors = new List<Error>();

            Results<VeaEvent> result = VeaEvent.CreateNewEvent();

            if (result.IsFailure)
                errors.AddRange(result.Errors);

            Results repoResult = await eventRepo.AddAsync(result.Value);

            if (repoResult.IsFailure)
                errors.AddRange(repoResult.Errors);

            if (errors.Any())
                return Results.Failure(errors.ToArray());

            await uow.SaveChangesAsync();

            return result;
        }
    }
}
