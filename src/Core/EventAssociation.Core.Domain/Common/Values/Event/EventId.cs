namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventId
    {
        public Guid Value { get; }
        public EventId()
        {
            Value = Guid.NewGuid();
        }
    }
}
