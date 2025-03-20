using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;

namespace UnitTests.Event
{
    public class EventTests
    {
        [Fact]
        public void Sucessfully_CreateEmptyEvent()
        {
            // Act
            var result = VeaEvent.CreateNewEvent().Value;

            // Assert
            Assert.True(result.status == EventStatus.Draft);
            Assert.Equal("Working Title", result.title.Value);
            Assert.Equal(5, result.Participants.MaxGuests);
            Assert.Equal("", result.Description.Value);
        }
}
}
