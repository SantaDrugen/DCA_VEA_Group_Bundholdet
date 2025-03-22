using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event
{ 
    // TODO: Set to public 
    internal class VeaEvent
    {
        public EventId Id { get; }
        public EventTitle? title { get; private set; }
        public EventDescription? Description { get; set; }
        public EventDateTime? DateTime { get; set; }
        private EventVisibility? Visibility { get; set; }
        public EventStatus? status { get; private set; }
        public EventParticipants? Participants { get; set; }
        private EventInvitations? Invitations { get; set; }

        public VeaEvent()
        {
            Id = new EventId();
        }

        public static Results<VeaEvent> CreateNewEvent()
        {
            var titleResult = EventTitle.Create("Working Title");
            if (titleResult.IsFailure)
            {
                return Results<VeaEvent>.Failure(titleResult.Errors.ToArray());
            }
            var eventDescription = EventDescription.Create("");

            var newEvent = new VeaEvent
            {
                status = EventStatus.Draft,
                title = titleResult.Value,
                Participants = new EventParticipants(maxGuests: 5),
                Description = eventDescription.Value,
                Visibility = EventVisibility.Private,
                // Probably needed:
                Invitations = new EventInvitations(),
            };
            return Results<VeaEvent>.Success(newEvent);
        }

        public static Results<VeaEvent> CreateNewEvent(string eventTitle)
        {
            var titleResult = EventTitle.Create(eventTitle);
            if (titleResult.IsFailure)
            {
                return Results<VeaEvent>.Failure(titleResult.Errors.ToArray());
            }

            var eventDescription = EventDescription.Create("");
            
            var newEvent = new VeaEvent
            {
                status = EventStatus.Draft,
                title = titleResult.Value,
                Participants = new EventParticipants(maxGuests: 5),
                Description = eventDescription.Value,
                Visibility = EventVisibility.Private,

                // Probably needed:
                Invitations = new EventInvitations(),

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
        
        public Results<EventStatus> SetReady()
        {
            // We have to evaluate the Results here, such that EventStatus is only updated if successful
            var result = EventStatus.SetReady(status);
            if (result.IsSuccess)
            {
                status = result.Value; 
            }
            return result;
        }
        
        public Results<EventDescription> SetDescription(string newDescription)
        {
            var descriptionResult = EventDescription.Create(newDescription);
            if (descriptionResult.IsFailure)
                return descriptionResult;

            if (status == EventStatus.Active)
            {
                return Results<EventDescription>.Failure(new Error("EVENT_ACTIVE", "An active event cannot be modified."));
            }

            if (status == EventStatus.Cancelled)
            {
                return Results<EventDescription>.Failure(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }
            
            Description = descriptionResult.Value;

            if (status == EventStatus.Ready)
                status = EventStatus.Draft;

            return Results<EventDescription>.Success(Description);
        }

        public Results<EventDateTime> SetDateTime(DateTime startDateTime, DateTime endDateTime)
        {
            var dateTimeResult = EventDateTime.Create(startDateTime, endDateTime);
            if (dateTimeResult.IsFailure)
                return dateTimeResult;
            if (status != EventStatus.Active)
            {
                return Results<EventDateTime>.Failure(new Error("EVENT_ACTIVE", "An active event cannot be modified."));
            }
            if (status == EventStatus.Cancelled)
            {
                return Results<EventDateTime>.Failure(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }
            if (status == EventStatus.Active)
            {
                return Results<EventDateTime>.Failure(new Error("EVENT_ACTIVE", "An active event cannot be modified."));
            }
            if (status == EventStatus.Created)
            {
                return Results<EventDateTime>.Failure(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }

            DateTime = dateTimeResult.Value;
            if (status == EventStatus.Ready)
                status = EventStatus.Draft;
            return Results<EventDateTime>.Success(DateTime);
        }
    }
}
