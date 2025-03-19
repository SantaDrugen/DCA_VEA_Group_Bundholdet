using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventDescription
    {
        private const int MaxLength = 250;

        public string Value { get; }

        private EventDescription(string description)
        {
            Value = description;
        }

        public static Results<EventDescription> Create(string description)
        {
            if (description.Length > MaxLength)
            {
                return Results<EventDescription>.Failure(new Error("DESCRIPTION_TOO_LONG",
                    "The description must be between 0 and 250 characters."));
            }

            return Results<EventDescription>.Success(new EventDescription(description));
        }
    }
}