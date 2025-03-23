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
        public EventDateTime? EventDateTime { get; set; }
        public EventVisibility? Visibility { get; set; }
        public EventStatus? status { get; private set; }
        public EventParticipants? Participants { get; set; }
        private EventInvitations? Invitations { get; set; }

        private static string defaultTitle = "Working Title";
        private static string defaultDescription = "";
        private static int defaultMaxGuests = 5;

        public VeaEvent()
        {
            Id = new EventId();
        }

        public static Results<VeaEvent> CreateNewEvent()
        {
            var titleResult = EventTitle.Create(defaultTitle);
            if (titleResult.IsFailure)
            {
                return Results<VeaEvent>.Failure(titleResult.Errors.ToArray());
            }
            var eventDescription = EventDescription.Create(defaultDescription);

            var newEvent = new VeaEvent
            {
                status = EventStatus.Draft,
                title = titleResult.Value,
                Participants = new EventParticipants(maxGuests: defaultMaxGuests),
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

            var eventDescription = EventDescription.Create(defaultDescription);

            var newEvent = new VeaEvent
            {
                status = EventStatus.Draft,
                title = titleResult.Value,
                Participants = new EventParticipants(maxGuests: defaultMaxGuests),
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

        public Results<EventDateTime> SetDateTime(EventDateTime eventDateTime)
        {
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

            if (status == EventStatus.Ready)
                status = EventStatus.Draft;

            EventDateTime = eventDateTime;
            return Results<EventDateTime>.Success(eventDateTime);
        }

        public Results<EventVisibility> SetVisibilityPublic()
        {
            if (status == EventStatus.Cancelled)
            {
                return Results<EventVisibility>.Failure(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }

            if (status == EventStatus.Ready && Visibility != EventVisibility.Public)
            {
                status = EventStatus.Draft;
            }

            Visibility = EventVisibility.Public;
            return Results<EventVisibility>.Success(Visibility);
        }

        public Results<EventVisibility> SetVisibilityPrivate()
        {
            var errors = new List<Error>();

            if (status == EventStatus.Cancelled)
            {
                errors.Add(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }

            if (status == EventStatus.Active)
            {
                errors.Add(new Error("EVENT_ACTIVE", "An active event cannot be modified."));
            }

            if (errors.Any())
            {
                return Results<EventVisibility>.Failure(errors.ToArray());
            }

            if (status == EventStatus.Ready && Visibility != EventVisibility.Private)
                status = EventStatus.Draft;

            Visibility = EventVisibility.Private;
            return Results<EventVisibility>.Success(Visibility);
        }

        public Results<int> SetMaxGuests(int maxGuests)
        {
            var errors = new List<Error>();

            if (status == EventStatus.Cancelled)
            {
                errors.Add(new Error("EVENT_CANCELLED", "A cancelled event cannot be modified."));
            }

            if (status == EventStatus.Active)
            {
                errors.Add(new Error("EVENT_ACTIVE", "An active event cannot be modified."));
            }

            if (errors.Any())
            {
                return Results<int>.Failure(errors.ToArray());
            }

            return Participants.SetMaxGuests(maxGuests);
        }

        public Results<EventStatus> SetEventStatusReady()
        {
            // Do we need to check that 'this' is a valid event (title, description, DateTime, visibility, maxGuests)?
            // Or is it possible to create an event without these properties being valid?

            var errors = new List<Error>();

            if (title?.Value == defaultTitle)
                errors.Add(new Error("EVENT_READY_STATUS", "Event must have a title."));

            if (Description?.Value == defaultDescription)
                errors.Add(new Error("EVENT_READY_STATUS", "Event must have a description."));

            if (EventDateTime == null)
                errors.Add(new Error("EVENT_READY_STATUS", "Event must have a date and time."));

            if (Visibility == null) // Can visibility be null? -- default is private -- Can a private event be ready?
                errors.Add(new Error("EVENT_READY_STATUS", "Event must have a visibility."));

            if (Participants?.MaxGuests < 5) // Might be redundant, as this is checked in SetMaxGuests
                errors.Add(new Error("EVENT_READY_STATUS", "Event must have at least 5 max guests."));

            if (Participants?.MaxGuests > 50) // Might be redundant, as this is checked in SetMaxGuests
                errors.Add(new Error("EVENT_READY_STATUS", "Event must have at most 50 max guests."));

            if (EventDateTime?.StartDateTime < DateTime.Now || EventDateTime?.EndDateTime < DateTime.Now)
                errors.Add(new Error("EVENT_READY_STATUS", "Cannot ready event in the past"));

            if (errors.Any())
                return Results<EventStatus>.Failure(errors.ToArray());

            var statusResult = EventStatus.SetReady(status);

            if (statusResult.Errors.Any())
            {
                return Results<EventStatus>.Failure(statusResult.Errors.ToArray());
            }

            status = statusResult.Value;

            return Results<EventStatus>.Success(status);
        }

        public Results<EventStatus> SetEventStatusActive()
        {
            var errors = new List<Error>();

            if (title?.Value == defaultTitle)
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Event must have a title."));

            if (Description?.Value == defaultDescription)
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Event must have a description."));

            if (EventDateTime == null)
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Event must have a date and time."));

            if (Visibility == null) // Can visibility be null? -- default is private -- Can a private event be ready?
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Event must have a visibility."));

            if (Participants?.MaxGuests < 5) // Might be redundant, as this is checked in SetMaxGuests
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Event must have at least 5 max guests."));

            if (Participants?.MaxGuests > 50) // Might be redundant, as this is checked in SetMaxGuests
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Event must have at most 50 max guests."));

            if (EventDateTime?.StartDateTime < DateTime.Now || EventDateTime?.EndDateTime < DateTime.Now)
                errors.Add(new Error("EVENT_ACTIVE_STATUS", "Cannot active event in the past"));

            if (errors.Any()) // If anything has failed, return now
                return Results<EventStatus>.Failure([.. errors]);

            if (status == EventStatus.Draft)
            {
                var readyResult = EventStatus.SetReady(status);

                if (readyResult.Errors.Any()) // If status update failed, return now
                {
                    return Results<EventStatus>.Failure(readyResult.Errors.ToArray());
                }
            }

            var activeResult = EventStatus.SetActive(status);

            if (activeResult.Errors.Any()) // If status update failed, return now
            {
                return Results<EventStatus>.Failure([.. activeResult.Errors]);
            }

            status = activeResult.Value;
            return Results<EventStatus>.Success(status);
        }
    }
}
