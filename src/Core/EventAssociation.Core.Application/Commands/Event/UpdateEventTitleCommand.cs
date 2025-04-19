using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class UpdateEventTitleCommand
    {
        public EventTitle newTtitle { get; }
        public EventId id { get; }

        private UpdateEventTitleCommand(EventTitle newTtitle, EventId id)
        {
            this.newTtitle = newTtitle;
            this.id = id;
        }

        public static Results<UpdateEventTitleCommand> Create(string id, string newTtitle)
        {
            Results<EventTitle> titleResult = EventTitle.Create(newTtitle);
            Results<EventId> idResult = EventId.FromString(id);

            List<Error> errors = new List<Error>();

            if (titleResult.IsFailure)
            {
                errors.AddRange(titleResult.Errors);
            }
            if (idResult.IsFailure)
            {
                errors.AddRange(idResult.Errors);
            }

            if (errors.Any())
            {
                return Results<UpdateEventTitleCommand>.Failure(errors.ToArray());
            }
            return Results<UpdateEventTitleCommand>.Success(new UpdateEventTitleCommand(titleResult.Value, idResult.Value));
        }
    }
}
