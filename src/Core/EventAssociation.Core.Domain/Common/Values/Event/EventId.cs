using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Event
{
    public class EventId
    {
        public Guid Value { get; set; }
        public EventId()
        {
            Value = Guid.NewGuid();
        }

        public static Results<EventId> FromString(string id)
        {
            var errors = new List<Error>();
            if (string.IsNullOrEmpty(id))
            {
                errors.Add(new Error("INVALID_EVENT_ID", "Event ID cannot be null or empty"));
            }
            if (!Guid.TryParse(id, out Guid parsedId))
            {
                errors.Add(new Error("INVALID_EVENT_ID", "Event ID is not a valid GUID"));
            }
            if (errors.Any())
            {
                return Results<EventId>.Failure(errors.ToArray());
            }
            return Results<EventId>.Success(new EventId { Value = parsedId });
        }

        public override bool Equals(object? obj)
        {
            if (obj is EventId other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
