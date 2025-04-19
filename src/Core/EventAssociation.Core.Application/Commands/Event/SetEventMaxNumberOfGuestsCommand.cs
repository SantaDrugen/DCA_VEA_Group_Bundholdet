using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class SetEventMaxNumberOfGuestsCommand
    {
        public EventId id { get; }
        public NumberOfGuests MaxNumberOfGuests { get; }
        private SetEventMaxNumberOfGuestsCommand(EventId eventId, NumberOfGuests maxNumberOfGuests)
        {
            id = eventId;
            MaxNumberOfGuests = maxNumberOfGuests;
        }
        public static Results<SetEventMaxNumberOfGuestsCommand> Create(string eventId, int maxNumberOfGuests)
        {
            List<Error> errors = new List<Error>();

            Results<EventId> idResult = EventId.FromString(eventId);

            Results<NumberOfGuests> guestsResult = NumberOfGuests.FromInt(maxNumberOfGuests);

            if (idResult.IsFailure)
                errors.AddRange(idResult.Errors);

            if (guestsResult.IsFailure)
                errors.AddRange(guestsResult.Errors);

            if (errors.Any())
                return Results<SetEventMaxNumberOfGuestsCommand>.Failure(errors.ToArray());

            return Results<SetEventMaxNumberOfGuestsCommand>.Success(new SetEventMaxNumberOfGuestsCommand(idResult.Value, guestsResult.Value));
        }
    }
}
