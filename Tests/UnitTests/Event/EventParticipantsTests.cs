using EventAssociation.Core.Domain.Common.Values.Event;

namespace UnitTests.Event
{
    public class EventParticipantsTests
    {
        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        public void SetMaxGuests_Success_WhenIntBetween5And50(int maxGuests)
        {
            // Arrange
            var eventParticipants = new EventParticipants(5);

            // Act
            var result = eventParticipants.SetMaxGuests(maxGuests);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(maxGuests, result.Value.Value);
        }

        [Fact]
        public void SetMaxGuests_Failure_WhenIntLessThan5()
        {
            // Arrange
            var eventParticipants = new EventParticipants(5);

            // Act
            var result = eventParticipants.SetMaxGuests(4);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INVALID_MAX_GUESTS", result.Errors.First().Code);
            Assert.Equal("Max guests must be at least 5", result.Errors.First().Message);
        }

        [Fact]
        public void SetMaxGuests_Failure_WhenIntGreaterThan50()
        {
            // Arrange
            var eventParticipants = new EventParticipants(5);

            // Act
            var result = eventParticipants.SetMaxGuests(51);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INVALID_MAX_GUESTS", result.Errors.First().Code);
            Assert.Equal("Max guests must be at most 50", result.Errors.First().Message);
        }


    }
}
