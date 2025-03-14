namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventVisibility : Enumeration
    {
        public static EventVisibility Public = new EventVisibility(1, "Public");
        public static EventVisibility Private = new EventVisibility(2, "Private");

        public EventVisibility(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
