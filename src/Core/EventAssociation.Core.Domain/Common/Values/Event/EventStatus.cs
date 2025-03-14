namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventStatus : Enumeration
    {
        public static EventStatus Draft = new EventStatus(0, "Draft");
        public static EventStatus Created = new EventStatus(1, "Created");
        public static EventStatus Ready = new EventStatus(2, "Ready");
        public static EventStatus Active = new EventStatus(3, "Active");

        public EventStatus(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
