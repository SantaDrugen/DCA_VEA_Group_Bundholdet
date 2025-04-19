using EventAssociation.Core.Tools.OperationResult;
using Error = EventAssociation.Core.Tools.OperationResult.Error;

namespace EventAssociation.Core.Domain.Common.Values.Event
{
    public class NumberOfGuests
    {
        public int Value { get; }
        private NumberOfGuests(int value)
        {
            Value = value;
        }

        public static Results<NumberOfGuests> FromInt(int value)
        {
            List<Error> errors = new List<Error>();

            if (value < 5)
            {
                errors.Add(new Error("INVALID_MAX_GUESTS", "Max guests must be at least 5"));
            }

            if (value > 50)
            {
                errors.Add(new Error("INVALID_MAX_GUESTS", "Max guests must be at most 50"));
            }

            if (errors.Any())
            {
                return Results<NumberOfGuests>.Failure(errors.ToArray());
            }

            return Results<NumberOfGuests>.Success(new NumberOfGuests(value));
        }
    }
}
