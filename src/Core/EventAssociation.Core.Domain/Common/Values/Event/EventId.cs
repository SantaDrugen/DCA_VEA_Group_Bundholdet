namespace EventAssociation.Core.Domain.Common.Values.Event
{
    public class EventId
    {
        public Guid Value { get; }
        public EventId()
        {
            Value = Guid.NewGuid();
        }
    }
}
