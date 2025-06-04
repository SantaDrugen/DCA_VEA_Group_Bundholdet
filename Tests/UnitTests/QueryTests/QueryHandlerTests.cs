using System.Text.Json;
using EfcQueries.QueryHandlers;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Queries;

namespace UnitTests.QueryTests
{
    public class QueryHandlerTests : IDisposable
    {

        public readonly VeaDbContext _writeContext;
        public readonly VeadatabaseProductionContext _readContext;
        public readonly GetEventDetailsQueryHandler _getEventDetailsQueryHandler;

        public QueryHandlerTests()
        {
            TestContextFactory.OpenInMemoryConnection();
            _writeContext = TestContextFactory.CreateWriteContext();
            _readContext = TestContextFactory.CreateReadContext();
            SeedJsonDataInto(_readContext);

            _getEventDetailsQueryHandler = new GetEventDetailsQueryHandler(_readContext);
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
            _writeContext.Dispose();
            _readContext.Dispose();
            TestContextFactory.CloseInMemoryConnection();
        }
    }
}
