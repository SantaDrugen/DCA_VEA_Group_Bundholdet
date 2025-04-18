using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event
{
    public class EventDateTime
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
            var errors = new List<Error>();

            if (startDateTime > endDateTime)
                errors.Add(new Error("INVALID_DATE_TIME", "The start date must be before the end date."));

            if (!((startDateTime.Date == endDateTime.Date) ||
                 (endDateTime.Date == startDateTime.Date.AddDays(1) && endDateTime.TimeOfDay < TimeSpan.FromHours(1))))
                errors.Add(new Error("INVALID_DATE_TIME", "The event must end at the latests 00:59 the day after it starts."));

            if ((endDateTime - startDateTime) < TimeSpan.FromHours(1))
                errors.Add(new Error("INVALID_DATE_TIME", "The event must last at least 1 hour."));

            if (startDateTime.TimeOfDay < TimeSpan.FromHours(1) && endDateTime.TimeOfDay > TimeSpan.FromHours(8))
                errors.Add(new Error("INVALID_DATE_TIME", "The event must not span the period between 01:00 and 08:00."));

            if (startDateTime.Hour < 8)
                errors.Add(new Error("INVALID_DATE_TIME", "The event must start after 8:00."));

            if (endDateTime.Hour >= 1 && endDateTime.Hour < 8)
                errors.Add(new Error("INVALID_DATE_TIME", "The event must not end between 01:00 and 08:00."));

            if (endDateTime.TimeOfDay - startDateTime.TimeOfDay > TimeSpan.FromHours(10))
                errors.Add(new Error("INVALID_DATE_TIME", "The event must last at most 10 hours."));

            if (startDateTime.Date < DateTime.Now.Date)
                errors.Add(new Error("INVALID_DATE_TIME", "The event must start in the future."));

            if (startDateTime.Date == DateTime.Now.Date && startDateTime.TimeOfDay < DateTime.Now.TimeOfDay)
                errors.Add(new Error("INVALID_DATE_TIME", "The event must start in the future."));

            if (endDateTime.Date < DateTime.Now.Date)
                errors.Add(new Error("INVALID_DATE_TIME", "The event must end in the future."));

            if (endDateTime.Date == DateTime.Now.Date && endDateTime.TimeOfDay < DateTime.Now.TimeOfDay)
                errors.Add(new Error("INVALID_DATE_TIME", "The event must end in the future."));

            return errors.Any()
                ? Results<EventDateTime>.Failure(errors.ToArray())
                : Results<EventDateTime>.Success(new EventDateTime(startDateTime, endDateTime));
        }

    }
}
