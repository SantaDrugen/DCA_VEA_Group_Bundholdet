using EventAssociation.Core.Domain.Common.Values.Guest;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    internal class EventParticipants
    {
        private int maxGuests;
        private List<GuestId> participants;

        public EventParticipants(int maxGuests)
        {
            this.maxGuests = maxGuests;
            this.participants = new List<GuestId>();
        }
    }
}
