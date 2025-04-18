using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    public class EventParticipants
    {
        public int MaxGuests;
        private List<GuestId> participants;

        public EventParticipants(int maxGuests)
        {
            this.MaxGuests = maxGuests;
            this.participants = new List<GuestId>();
        }

        public Results<int> SetMaxGuests(int maxGuests)
        {
            var errors = new List<Error>();

            if (maxGuests < 5)
            {
                errors.Add(new Error("INVALID_MAX_GUESTS", "Max guests must be at least 5"));
            }

            if (maxGuests > 50)
            {
                errors.Add(new Error("INVALID_MAX_GUESTS", "Max guests must be at most 50"));
            }

            if (errors.Any())
            {
                return Results<int>.Failure(errors.ToArray());
            }

            MaxGuests = maxGuests;
            return Results<int>.Success(MaxGuests);
        }
    }
}
