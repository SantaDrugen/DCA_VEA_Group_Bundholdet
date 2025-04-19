using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class SetEventStatusActiveCommand
    {
        public EventId id;

        private SetEventStatusActiveCommand(EventId id)
        {
            this.id = id;
        }

        public static Results<SetEventStatusActiveCommand> Create(string id)
        {
            List<Error> errors = new List<Error>();

            Results<EventId> eventIdResult = EventId.FromString(id);

            if (eventIdResult.IsFailure)
                errors.AddRange(eventIdResult.Errors);

            if (errors.Any())
                return Results<SetEventStatusActiveCommand>.Failure(errors.ToArray());

            return Results<SetEventStatusActiveCommand>.Success(new SetEventStatusActiveCommand(eventIdResult.Value));
        }
    }
}
