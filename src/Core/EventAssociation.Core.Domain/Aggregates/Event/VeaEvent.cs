using EventAssociation.Core.Domain.Common.Values.Event;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    internal class VeaEvent
    {
        public EventId Id { get; }
        private EventTitle title { get; set; }
        private EventDescription description { get; set; }
        private DateTime startDateTime { get; set; }
        private DateTime endDateTime { get; set; }
        private EventVisibility visibility { get; set; }
        private EventStatus status { get; set; }
        private EventParticipants participants { get; set; }
        private EventInvitations invitations { get; set; }

        public VeaEvent()
        {
            this.Id = new EventId();
        }

        public static VeaEvent CreateNewEvent()
        {
            var newEvent = new VeaEvent
            {
                status = EventStatus.Draft,
                participants = new EventParticipants(maxGuests: 5),
                title = new EventTitle("Working Title"),
                description = new EventDescription(""),
                visibility = EventVisibility.Private,

                // Probably needed:
                invitations = new EventInvitations(),

            };

            return newEvent;
        }
    }
}
