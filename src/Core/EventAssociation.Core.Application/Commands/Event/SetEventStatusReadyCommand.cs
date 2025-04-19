using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class SetEventStatusReadyCommand
    {
        public EventId id { get; }

        private SetEventStatusReadyCommand(EventId id)
        {
            this.id = id;
        }

        public static Results<SetEventStatusReadyCommand> Create(string id)
        {
            List<Error> errors = new List<Error>();

            Results<EventId> idResult = EventId.FromString(id);

            if (idResult.IsFailure)
                errors.AddRange(idResult.Errors);

            if (errors.Any())
                return Results<SetEventStatusReadyCommand>.Failure(errors.ToArray());

            return Results<SetEventStatusReadyCommand>.Success(new SetEventStatusReadyCommand(idResult.Value));
        }
    }
}
