using EventAssociation.Core.Domain.Aggregates.Event;

namespace UnitTests.Event
{
    public class DateTimeTests
    {
        private DateTime today = DateTime.Now;
        private DateTime date7AM;
        private DateTime date8AM;
        private DateTime date830AM;
        private DateTime date6PM;
        private DateTime date7PM;
        private DateTime date11PM;
        private DateTime date12Am;
        private DateTime date0059AM;
        private DateTime date01AM;
        private DateTime nextDay2PM;

        private DateTime futureDate7AM;
        private DateTime futureDate8AM;
        private DateTime futureDate6PM;
        private DateTime futureDateNextDay2AM;

        private DateTime pastDate8AM;
        private DateTime pastDate6PM;

        public DateTimeTests()
        {
            date7AM = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0).AddDays(1);
            date8AM = new DateTime(today.Year, today.Month, today.Day, 8, 0, 0).AddDays(1);
            date830AM = new DateTime(today.Year, today.Month, today.Day, 8, 30, 0).AddDays(1);
            date6PM = new DateTime(today.Year, today.Month, today.Day, 18, 0, 0).AddDays(1);
            date7PM = new DateTime(today.Year, today.Month, today.Day, 19, 0, 0).AddDays(1);
            date11PM = new DateTime(today.Year, today.Month, today.Day, 23, 0, 0).AddDays(1);
            date12Am = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0).AddDays(2);
            date0059AM = new DateTime(today.Year, today.Month, today.Day, 0, 59, 0).AddDays(2);
            date01AM = new DateTime(today.Year, today.Month, today.Day, 1, 0, 0).AddDays(2);
            nextDay2PM = new DateTime(today.Year, today.Month, today.Day, 2, 0, 0).AddDays(2);

            futureDate7AM = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0).AddDays(1);
            futureDate8AM = new DateTime(today.Year, today.Month, today.Day, 8, 0, 0).AddDays(1);
            futureDate6PM = new DateTime(today.Year, today.Month, today.Day, 18, 0, 0).AddDays(1);
            futureDateNextDay2AM = new DateTime(today.Year, today.Month, today.Day, 2, 0, 0).AddDays(2);
        }

        [Fact]
        public void DateTime_Success_WhenSunnyScenario()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date8AM, date6PM);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(date8AM, result.Value.StartDateTime);
            Assert.Equal(date6PM, result.Value.EndDateTime);
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
        public void DateTime_Success_WhenEndIs1DayLaterAndBefore0100()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date8AM, date0059AM);

            Assert.True(result.IsSuccess);
            Assert.Equal(date8AM, result.Value.StartDateTime);
            Assert.Equal(date0059AM, result.Value.EndDateTime);
        }

        [Fact]
        public void DateTime_Fails_WhenEndIs1DayLaterAndAfter0100()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date8AM, date01AM);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Success_WhenSunnyScenarioInFuture()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(futureDate8AM, futureDate6PM);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(futureDate8AM, result.Value.StartDateTime);
            Assert.Equal(futureDate6PM, result.Value.EndDateTime);
        }

        [Fact]
        public void DateTime_Fails_WhenInvalidStartOrEndInFuture()
        {
            // Arrange
            var eventDateTime1 = EventDateTime.Create();
            var eventDateTime2 = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(futureDate7AM, futureDate6PM);
            var result2 = EventDateTime.Create(futureDate6PM, futureDateNextDay2AM);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
            Assert.True(result2.IsFailure);
            Assert.Contains(result2.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Fails_WhenEventLongerThan10Hours()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date8AM, date7PM);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Success_WhenDurationIsExactly10Hours()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date8AM, date6PM);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(date8AM, result.Value.StartDateTime);
            Assert.Equal(date6PM, result.Value.EndDateTime);
        }

        [Fact]
        public void DateTime_Fails_WhenStartDateAndEndDateInPast()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(pastDate8AM, pastDate6PM);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }

        [Fact]
        public void DateTime_Fails_WhenEventSpansBetween0100And0800()
        {
            // Arrange
            var eventDateTime = EventDateTime.Create();

            // Act
            var result = EventDateTime.Create(date01AM, futureDate8AM);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Code == "INVALID_DATE_TIME");
        }
    }
}
