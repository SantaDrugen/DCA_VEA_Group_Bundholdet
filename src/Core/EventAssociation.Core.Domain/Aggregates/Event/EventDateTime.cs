using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    internal class EventDateTime
    {
        public DateTime? StartDateTime { get; }
        public DateTime? EndDateTime { get; }

        public EventDateTime(DateTime? startDateTime, DateTime? endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }

        public static Results<EventDateTime> Create()
        {
            return Results<EventDateTime>.Success(new EventDateTime(null, null));
        }

        public static Results<EventDateTime> Create(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime) // end before start
            {
                return Results<EventDateTime>.Failure(new Error("INVALID_DATE_TIME",
                    "The start date must be before the end date."));
            }
            else if (startDateTime.Date != endDateTime.Date) // not same day
            {
                return Results<EventDateTime>.Failure(new Error("INVALID_DATE_TIME",
                    "The start and end date must be on the same day."));
            }
            else if ((endDateTime - startDateTime) < TimeSpan.FromHours(1)) // less than 1 hour
            {
                return Results<EventDateTime>.Failure(new Error("INVALID_DATE_TIME",
                    "The event must last at least 1 hour."));
            }
            else if (startDateTime.Hour < 8) // start before 8
            {
                return Results<EventDateTime>.Failure(new Error("INVALID_DATE_TIME",
                    "The event must start after 8:00."));
            }
            else if (endDateTime.Hour >= 24) // end after 24
            {
                return Results<EventDateTime>.Failure(new Error("INVALID_DATE_TIME",
                    "The event must end before 24:00."));
            }
            return Results<EventDateTime>.Success(new EventDateTime(startDateTime, endDateTime));
        }
    }
}
