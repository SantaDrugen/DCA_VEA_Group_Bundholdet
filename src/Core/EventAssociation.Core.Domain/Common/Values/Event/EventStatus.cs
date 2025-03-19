using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventStatus : Enumeration
    {
        public static EventStatus Draft = new EventStatus(0, "Draft");
        public static EventStatus Created = new EventStatus(1, "Created");
        public static EventStatus Ready = new EventStatus(2, "Ready");
        public static EventStatus Active = new EventStatus(3, "Active");
        public static EventStatus Cancelled = new EventStatus(4, "Cancelled");

        public EventStatus(int value, string displayName) : base(value, displayName)
        {
        }


        public static Results<EventStatus> SetActive(EventStatus current)
        {
            if (current == Cancelled)
                return Results<EventStatus>.Failure(new Error("EVENT_CANCELLED", "Cannot active a cancelled event."));
            
            return Results<EventStatus>.Success(Active);
        }
        
        public static Results<EventStatus> SetCancelled()
        {
            return Results<EventStatus>.Success(Cancelled);
        }
        
        public static Results<EventStatus> SetReady(EventStatus current)
        {
            if (current == Cancelled)
                return Results<EventStatus>.Failure(new Error("EVENT_CANCELLED", "Cannot ready a cancelled event."));
            
            if (current == Active)
                return Results<EventStatus>.Failure(new Error("EVENT_ACTIVE", "Cannot ready an active event."));
            
            return Results<EventStatus>.Success(Ready);
        }
    }
}
