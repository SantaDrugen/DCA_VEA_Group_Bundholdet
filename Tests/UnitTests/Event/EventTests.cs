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
            Assert.Equal(5, result.Participants.MaxGuests.Value);
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
            Assert.Equal(10, veaEvent.Participants.MaxGuests.Value);
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

        [Fact]
        public void EventStatusSetReady_Success_WhenCurrentStatusIsDraftAndEventHasAllRequiredValues()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetDescription("Description");
            veaEvent.SetTitle("Valid Title");

            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(1);
            var startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
            var endDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 17, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDateTime, endDateTime).Value;

            veaEvent.SetDateTime(eventDateTime);

            // Act
            var result = veaEvent.SetEventStatusReady();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Ready, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetReady_Fails_WhenCurrentStatusIsDraftAndEventStillHasDefaultTitle()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetDescription("Description");

            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(1);
            var startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
            var endDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 17, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDateTime, endDateTime).Value;

            veaEvent.SetDateTime(eventDateTime);

            // Act
            var result = veaEvent.SetEventStatusReady();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetReady_Fails_WhenCurrentStatusIsDraftAndEventStillHasDefaultDescription()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");

            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(1);
            var startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
            var endDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 17, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDateTime, endDateTime).Value;

            veaEvent.SetDateTime(eventDateTime);

            // Act
            var result = veaEvent.SetEventStatusReady();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetReady_Fails_WhenCurrentStatusIsDraftAndEventStillHasDefaultDateTime()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            // Act
            var result = veaEvent.SetEventStatusReady();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetReady_Fails_WhenCurrentStatusIsDraftAndEventIsInThePast() // If run between 00.00 and 08.00 this test will fail
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = DateTime.Now.AddSeconds(1);
            var endDate = DateTime.Now.AddHours(1).AddSeconds(2);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            
            veaEvent.SetDateTime(eventDateTime);

            Thread.Sleep(2000);

            // Act
            var result = veaEvent.SetEventStatusReady();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventDateTime_Success_ChangesStatusFromReadyToDraft()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = new DateTime(4000, 10, 10, 8, 0, 0);
            var endDate = new DateTime(4000, 10, 10, 18, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);
            veaEvent.SetEventStatusReady();

            startDate = startDate.AddDays(1);
            endDate = endDate.AddDays(1);
            eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);

            // Act
            var result = veaEvent.SetDateTime(eventDateTime);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventVisibilitySetPrivate_Success_WhenStatusIsReadyAndAlreadyPrivate()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = new DateTime(4000, 10, 10, 8, 0, 0);
            var endDate = new DateTime(4000, 10, 10, 18, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);

            veaEvent.SetEventStatusReady();

            // Act
            veaEvent.SetVisibilityPrivate();

            // Assert
            Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
            Assert.Equal(EventStatus.Ready, veaEvent.status);
        }

        [Fact]
        public void EventVisibilitySetPrivate_Success_WhenStatusIsDraftAndAlreadyPrivate()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;

            // Act
            veaEvent.SetVisibilityPrivate();

            // Assert
            Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventVisibilitySetPublic_Success_WhenStatusIsDraftAndVisibilityPrivate()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPrivate();

            // Act
            veaEvent.SetVisibilityPublic();

            // Assert
            Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventVisibilitySetPublic_Success_WhenStatusIsReadyAndVisibilityPrivate()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = new DateTime(4000, 10, 10, 8, 0, 0);
            var endDate = new DateTime(4000, 10, 10, 18, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);

            veaEvent.SetEventStatusReady();

            // Act
            var result = veaEvent.SetVisibilityPublic();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetActive_Success_WhenEventIsViableAndStatusIsDraft()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = new DateTime(4000, 10, 10, 8, 0, 0);
            var endDate = new DateTime(4000, 10, 10, 18, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);

            // Act
            var result = veaEvent.SetEventStatusActive();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Active, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetActive_Success_WhenStatusIsReady()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = new DateTime(4000, 10, 10, 8, 0, 0);
            var endDate = new DateTime(4000, 10, 10, 18, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);

            veaEvent.SetEventStatusReady();

            // Act
            var result = veaEvent.SetEventStatusActive();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Active, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetActive_Success_WhenStatusIsAlreadyActive()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;
            veaEvent.SetVisibilityPublic();
            veaEvent.SetMaxGuests(10);
            veaEvent.SetTitle("Valid Title");
            veaEvent.SetDescription("Description");

            var startDate = new DateTime(4000, 10, 10, 8, 0, 0);
            var endDate = new DateTime(4000, 10, 10, 18, 0, 0);
            EventDateTime eventDateTime = EventDateTime.Create(startDate, endDate).Value;
            veaEvent.SetDateTime(eventDateTime);

            veaEvent.SetEventStatusActive();

            // Act
            var result = veaEvent.SetEventStatusActive();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Active, veaEvent.status);
        }

        [Fact]
        public void EventStatusSetActive_Failure_WhenStatusIsDraftAndEventNotViable()
        {
            // Arrange
            var veaEvent = VeaEvent.CreateNewEvent().Value;

            // Act
            var result = veaEvent.SetEventStatusActive();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(EventStatus.Draft, veaEvent.status);
            
            // Assert that all the expected errors are present
            Assert.Equal(3, result.Errors.Count);

            Assert.Equal("Event must have a title.", result.Errors[0].Message);
            Assert.Equal("Event must have a description.", result.Errors[1].Message);
            Assert.Equal("Event must have a date and time.", result.Errors[2].Message);
        }
    }
}
