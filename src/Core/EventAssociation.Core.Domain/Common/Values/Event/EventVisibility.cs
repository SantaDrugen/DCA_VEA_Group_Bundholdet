namespace EventAssociation.Core.Domain.Common.Values.Event
{
    public class EventVisibility : Enumeration
    {
        public static EventVisibility Public = new EventVisibility(1, "Public");
        public static EventVisibility Private = new EventVisibility(2, "Private");

        public EventVisibility()
        {

        }

        public EventVisibility(int value, string displayName) : base(value, displayName)
        {
        }

        public static EventVisibility From(int? value)
        {
            return GetAll<EventVisibility>().FirstOrDefault(v => v.Value == value)
                   ?? throw new ArgumentException($"No EventVisibility found for value {value}");
        }
    }
}
