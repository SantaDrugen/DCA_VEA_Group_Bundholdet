using EventAssociation.Core.Domain.Common.Values.Event;

namespace UnitTests.Event
{
    public class EventStatusTests
    {
        [Fact]
        public void SetReady_Success_WhenCurrentIsDraft()
        {
            // Arrange
            var current = EventStatus.Draft;

            // Act
            var result = EventStatus.SetReady(current);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Ready, result.Value);
        }
    }
}
