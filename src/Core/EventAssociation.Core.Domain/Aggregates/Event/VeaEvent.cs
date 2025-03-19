using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    internal class VeaEvent
    {
        public EventId Id { get; }
        public EventTitle title { get; private set; }
        private EventDescription description { get; set; }
        private DateTime startDateTime { get; set; }
        private DateTime endDateTime { get; set; }
        private EventVisibility visibility { get; set; }
        public EventStatus status { get; private set; }
        private EventParticipants participants { get; set; }
        private EventInvitations invitations { get; set; }

        public VeaEvent()
        {
            this.Id = new EventId();
        }

        public static Results<VeaEvent> CreateNewEvent(string eventTitle)
        {
            var titleResult = EventTitle.Create(eventTitle);
            if (titleResult.IsFailure)
            {
                return Results<VeaEvent>.Failure(titleResult.Errors.ToArray());
            }
            
            var newEvent = new VeaEvent
            {
                status = EventStatus.Draft,
                title = titleResult.Value,
                participants = new EventParticipants(maxGuests: 5),
                description = new EventDescription(""),
                visibility = EventVisibility.Private,

                // Probably needed:
                invitations = new EventInvitations(),

            };

            return Results<VeaEvent>.Success(newEvent);
        }
        
        
        // Some verification is easier done here, as Event knows its own state.
        public Results<EventTitle> SetTitle(string newTitle)
        {
            var titleResult = EventTitle.Create(newTitle);
            if (titleResult.IsFailure)
                return titleResult;

            if (status == EventStatus.Active)
            {
                Console.WriteLine("EVENT ACTIVE");
                return Results<EventTitle>.Failure(new Error("EVENT_ACTIVE", "An active event cannot be modified."));
            }
            
            if (status == EventStatus.Cancelled)
            {
                Console.WriteLine("EVENT CANCELLED");
                return Results<EventTitle>.Failure(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }
                

            title = titleResult.Value;

            if (status == EventStatus.Ready)
                status = EventStatus.Draft;

            return Results<EventTitle>.Success(title);
        }

        public Results<EventStatus> SetActive()
        {
            // We have to evaluate the Results here, such that EventStatus is only updated if successful
            var result = EventStatus.SetActive(status);
            if (result.IsSuccess)
            {
                status = result.Value;
            }
            return result;
        }

        public Results<EventStatus> SetCancelled()
        {
            // We have to evaluate the Results here, such that EventStatus is only updated if successful
            var result = EventStatus.SetCancelled();
            if (result.IsSuccess)
            {
                status = result.Value; 
            }
            return result;
        }
    }
}
