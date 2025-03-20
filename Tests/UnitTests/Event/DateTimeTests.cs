using EventAssociation.Core.Domain.Aggregates.Event;

namespace UnitTests.Event
{
    public class DateTimeTests
    {
        private DateTime today = DateTime.Now;
        private DateTime date7AM;
        private DateTime date8AM;
        private DateTime date830AM;
        private DateTime date11PM;
        private DateTime date12Am;
        private DateTime nextDay2PM;

        public DateTimeTests()
        {
            date7AM = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0);
            date8AM = new DateTime(today.Year, today.Month, today.Day, 8, 0, 0);
            date830AM = new DateTime(today.Year, today.Month, today.Day, 8, 30, 0);
            date11PM = new DateTime(today.Year, today.Month, today.Day, 23, 0, 0);
            date12Am = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0).AddDays(1);
            nextDay2PM = new DateTime(today.Year, today.Month, today.Day, 2, 0, 0).AddDays(1);
        }

        [Fact]
        public void DateTime_Success_WhenStartBeforeEndAndOnSameDateWithinAllowedTime()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date8AM, date11PM);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(date8AM, result.Value.StartDateTime);
            Assert.Equal(date11PM, result.Value.EndDateTime);
        }

        [Fact]
        public void DateTime_Fails_WhenStartAfterEnd()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();
            // Act
            var result = EventDateTime.Create(date11PM, date8AM);
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Fails_WhenStartAndEndNotOnSameDate()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();
            // Act
            var result = EventDateTime.Create(date8AM, nextDay2PM);
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Fails_WhenStartAndEndAreLessThan1HourApart()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();
            // Act
            var result = EventDateTime.Create(date8AM, date830AM);
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTIme_Fails_WhenStartIsBefore8AM()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();
            // Act
            var result = EventDateTime.Create(date7AM, date11PM);
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Fails_WhenEndIsAfter12AM()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();
            // Act
            var result = EventDateTime.Create(date8AM, date12Am);
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }
    }
}
