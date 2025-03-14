using EventAssociation.Core.Domain.Common.Values.Event;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    internal class VeaEvent
    {
        public EventId Id { get; }
        private EventTitle title { get; set; }
        private EventDescription description { get; set; }
        private DateTime startDateTime { get; }
        private DateTime endDateTime { get; }
        private EventVisibility visibility { get; set; }
        private EventStatus status { get; set; }
        private EventParticipants participants { get; set; }
        private EventInvitations invitations { get; }

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
            };

            return newEvent;
        }
    }
}
