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

        [Fact]
        public void EventVisibilityUpdated_Success_WhenStatusIsDraft()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;

            // Act
            veaEvent.SetVisibilityPublic();

            // Assert
            Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventVisibilityUpdated_Success_WhenPublicEventIsSetPrivate()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();

            // Act
            veaEvent.SetVisibilityPrivate();

            // Assert
            Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventMaxGuestsUpdated_Success_WhenStatusIsDraft()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;

            // Act
            veaEvent.SetMaxGuests(10);

            // Assert
            Assert.Equal(10, veaEvent.Participants.MaxGuests);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventMaxGuestsUpdated_Success_WhenCurrentStatusIsReady()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void EventMaxGuestsUpdated_Success_WhenCurrentStatusIsActive()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void EventMaxGuestsUpdated_Failure_WhenStatusIsActiveAndNewMaxGuestsIsLessThanCurrent()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void EventMaxGuestsUpdated_Failure_WhenStatusIsCancelled()
        {
            throw new NotImplementedException();
        }
    }
}
