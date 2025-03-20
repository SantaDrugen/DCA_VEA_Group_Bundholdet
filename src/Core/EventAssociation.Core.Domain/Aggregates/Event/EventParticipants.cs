using EventAssociation.Core.Domain.Common.Values.Guest;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    internal class EventParticipants
    {
        public int MaxGuests;
        private List<GuestId> participants;

        public EventParticipants(int maxGuests)
        {
            this.MaxGuests = maxGuests;
            this.participants = new List<GuestId>();
        }
    }
}
