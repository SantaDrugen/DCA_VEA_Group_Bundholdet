using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class SetEventPublicCommand
    {
        public EventId id { get; }

        private SetEventPublicCommand(EventId id)
        {
            this.id = id;
        }

        public static Results<SetEventPublicCommand> Create(string id)
        {
            Results<EventId> idResult = EventId.FromString(id);

            List<Error> errors = new List<Error>();

            if (idResult.IsFailure)
            {
                errors.AddRange(idResult.Errors);
            }
            if (errors.Any())
            {
                return Results<SetEventPublicCommand>.Failure(errors.ToArray());
            }

            return Results<SetEventPublicCommand>.Success(new SetEventPublicCommand(idResult.Value));
        }
    }
}
