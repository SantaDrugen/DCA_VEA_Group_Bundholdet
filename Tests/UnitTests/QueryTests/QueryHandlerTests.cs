using System.Text.Json;
using EfcQueries.Models;
using EfcQueries.Queries;
using EfcQueries.QueryHandlers;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.QueryTests
{
    public class QueryHandlerTests : IDisposable
    {
        public readonly VeadatabaseProductionContext _readContext;

        public readonly GetEventDetailsQueryHandler _getEventDetailsQueryHandler;
        public readonly GetGuestProfileQueryHandler _getGuestProfileQueryHandler;
        public readonly GetUpcomingEventsQueryHandler _getUpcomingEventsQueryHandler;
        public readonly GetUnpublishedEventsQueryHandler _getUnpublishedEventsQueryHandler;

        public QueryHandlerTests()
        {
            TestContextFactory.OpenInMemoryConnection();
            _readContext = TestContextFactory.CreateReadContext();
            SeedJsonDataInto(_readContext);

            _getEventDetailsQueryHandler = new GetEventDetailsQueryHandler(_readContext);
            _getGuestProfileQueryHandler = new GetGuestProfileQueryHandler(_readContext);
            _getUpcomingEventsQueryHandler = new GetUpcomingEventsQueryHandler(_readContext);
            _getUnpublishedEventsQueryHandler = new GetUnpublishedEventsQueryHandler(_readContext);
        }

        [Fact]
        public async Task GetEventDetails_ReturnsInCorrectFormat_WriteToFile()
        {
            var query = new GetEventDetailsQuery("78f3ae84-f979-4599-a32b-45d51ec1598c", 0, 0);

            var veaEvent = await _getEventDetailsQueryHandler.HandleAsync(query);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(veaEvent, options);

            var outputPath = Path.Combine(AppContext.BaseDirectory, "EventDetails.json");
            File.WriteAllText(outputPath, json);
        }

        [Fact]
        public async Task GetGuestProfile_ReturnsInCorrectFormat_WriteToFile()
        {
            var query = new GetGuestProfileQuery("da804bb6-c593-4ce1-ab84-6fc10302ec53");

            var profile = await _getGuestProfileQueryHandler.HandleAsync(query);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(profile, options);

            var outputPath = Path.Combine(AppContext.BaseDirectory, "GuestProfile.json");
            File.WriteAllText(outputPath, json);
        }

        [Fact]
        public async Task GetUpcomingEvents_ReturnsInCorrectFormat_WriteToFile()
        {
            var query = new GetUpcomingEventsQuery(1, 5);

            var upcomingEvents = await _getUpcomingEventsQueryHandler.HandleAsync(query);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(upcomingEvents, options);

            var outputPath = Path.Combine(AppContext.BaseDirectory, "UpcomingEvents.json");
            File.WriteAllText(outputPath, json);
        }

        [Fact]
        public async Task GetUnpublishedEvents_ReturnsInCorrectFormat_WriteToFile()
        {
            var query = new GetUnpublishedEventsQuery(1, 5);

            var unpublishedEvents = await _getUnpublishedEventsQueryHandler.HandleAsync(query);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(unpublishedEvents, options);

            var outputPath = Path.Combine(AppContext.BaseDirectory, "UnpublishedEvents.json");
            File.WriteAllText(outputPath, json);
        }

        public static void SeedJsonDataInto(VeadatabaseProductionContext readContext)
        {
            readContext.Database.ExecuteSqlRaw("DELETE FROM VeaEvents");
            readContext.Database.ExecuteSqlRaw("DELETE FROM VeaGuests");
            readContext.Database.ExecuteSqlRaw("DELETE FROM EventGuests");

            var events = JsonTestDataLoader.LoadEntities<VeaEvent>("Events.json");
            if (events != null)
            {
                readContext.VeaEvents.AddRange(events);
            }

            var guests = JsonTestDataLoader.LoadEntities<VeaGuest>("Guests.json");
            if (guests != null)
            {
                readContext.VeaGuests.AddRange(guests);
            }

            var eventGuests = JsonTestDataLoader.LoadEntities<EventGuest>("Participations.json");
            if (eventGuests != null)
            {
                readContext.EventGuests.AddRange(eventGuests);
            }

            readContext.SaveChanges();
        }


        public void Dispose()
        {
            _readContext.Dispose();
            TestContextFactory.CloseInMemoryConnection();
        }
    }
}
