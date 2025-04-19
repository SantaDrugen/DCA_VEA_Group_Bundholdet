using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class SetEventPrivateCommand
    {
        public EventId id { get; }

        private SetEventPrivateCommand(EventId id)
        {
            this.id = id;
        }

        public static Results<SetEventPrivateCommand> Create(string id)
        {
            Results<EventId> idResult = EventId.FromString(id);

            List<Error> errors = new List<Error>();

            if (idResult.IsFailure)
            {
                errors.AddRange(idResult.Errors);
            }

            if (errors.Any())
            {
                return Results<SetEventPrivateCommand>.Failure(errors.ToArray());
            }

            return Results<SetEventPrivateCommand>.Success(new SetEventPrivateCommand(idResult.Value));
        }
    }
}
