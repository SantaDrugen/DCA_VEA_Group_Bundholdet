using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class UpdateEventDateTimeCommand
    {
        public EventId id { get; }
        public EventDateTime newDateTime { get; }

        private UpdateEventDateTimeCommand(EventId id, EventDateTime newDateTime)
        {
            this.id = id;
            this.newDateTime = newDateTime;
        }

        public static Results<UpdateEventDateTimeCommand> Create(string id, DateTime newDateTimeStart, DateTime newDateTimeEnd)
        {
            Results<EventId> idResult = EventId.FromString(id);
            Results<EventDateTime> dateTimeResult = EventDateTime.Create(newDateTimeStart, newDateTimeEnd);

            List<Error> errors = new List<Error>();

            if (idResult.IsFailure)
            {
                errors.AddRange(idResult.Errors);
            }
            if (dateTimeResult.IsFailure)
            {
                errors.AddRange(dateTimeResult.Errors);
            }
            if (errors.Any())
            {
                return Results<UpdateEventDateTimeCommand>.Failure(errors.ToArray());
            }

            return Results<UpdateEventDateTimeCommand>.Success(new UpdateEventDateTimeCommand(idResult.Value, dateTimeResult.Value));
        }
    }
}
