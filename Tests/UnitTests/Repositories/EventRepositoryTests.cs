using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Database.Helpers;

namespace UnitTests.Repositories
{
    public class EventRepositoryTests
    {
        [Fact]
        public async Task AddAsync_SavesAndRetrievesEvent_WithAllValueObjects()
        {
            // Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new EventRepository(context);

            // Create a new VeaEvent (domain factory)
            var createResult = VeaEvent.CreateNewEvent("Integration Test Event");
            Assert.True(createResult.IsSuccess, "Failed to create a new VeaEvent.");
            var @event = createResult.Value;

            // Modify some properties before saving
            var descResult = @event.SetDescription("This is a test description.");
            Assert.True(descResult.IsSuccess, "Failed to set description.");

            var dateTimeValue = EventDateTime.Create(
                startDateTime: DateTime.UtcNow.AddDays(1),
                endDateTime: DateTime.UtcNow.AddDays(1).AddHours(2)
            ).Value;
            var dtResult = @event.SetDateTime(dateTimeValue);
            Assert.True(dtResult.IsSuccess, "Failed to set date/time.");

            var visResult = @event.SetVisibilityPublic();
            Assert.True(visResult.IsSuccess, "Failed to set visibility to Public.");

            // Act: add and commit
            await repository.AddAsync(@event);
            await context.SaveChangesAsync();

            // Retrieve by the underlying GUID
            var loadedResult = await repository.GetAsync(@event.Id);
            var loaded = loadedResult.Value;
            Assert.NotNull(loaded);

            // Assert: all properties and value objects persist correctly
            Assert.Equal(@event.Id.Value, loaded.Id.Value);
            Assert.Equal("Integration Test Event", loaded.title!.Value);
            Assert.Equal("This is a test description.", loaded.Description!.Value);
            Assert.Equal(dateTimeValue.StartDateTime, loaded.EventDateTime!.StartDateTime);
            Assert.Equal(dateTimeValue.EndDateTime, loaded.EventDateTime.EndDateTime);
            Assert.Equal(EventVisibility.Public, loaded.Visibility);
            Assert.Equal(EventStatus.Draft, loaded.status); // Default status remains Draft

            // Cleanup
            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task AddAsync_NullEvent_ThrowsArgumentNullException()
        {
            // Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new EventRepository(context);

            // Act & Assert: passing null should throw ArgumentNullException
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // NOTE: “null!” is used to bypass the compiler’s null‐safety here
                await repository.AddAsync(null!);
            });

            // Cleanup
            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task RemoveAsync_DeletesEvent_FromDatabase()
        {
            // Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new EventRepository(context);

            // Create and save a new VeaEvent
            var createResult = VeaEvent.CreateNewEvent("Event To Remove");
            Assert.True(createResult.IsSuccess, "Failed to create a new VeaEvent.");
            var @event = createResult.Value;

            var descResult = @event.SetDescription("Description before removal.");
            Assert.True(descResult.IsSuccess, "Failed to set description.");

            var dateTimeValue = EventDateTime.Create(
                startDateTime: DateTime.UtcNow.AddDays(2),
                endDateTime: DateTime.UtcNow.AddDays(2).AddHours(3)
            ).Value;
            var dtResult = @event.SetDateTime(dateTimeValue);
            Assert.True(dtResult.IsSuccess, "Failed to set date/time.");

            var visResult = @event.SetVisibilityPublic();
            Assert.True(visResult.IsSuccess, "Failed to set visibility.");

            await repository.AddAsync(@event);
            await context.SaveChangesAsync();

            // Act: remove the event
            var removeResult = await repository.RemoveAsync(@event.Id);
            Assert.True(removeResult.IsSuccess, "RemoveAsync returned failure.");
            await context.SaveChangesAsync();

            // Assert: attempting to fetch it again yields failure
            var loadedResult = await repository.GetAsync(@event.Id);
            Assert.True(loadedResult.IsFailure, "GetAsync should fail for a removed entity.");
            Assert.Contains(
                loadedResult.Errors,
                e => e.Code == "DATABASE_ERROR" && e.Message.Contains("not found")
            );

            // Cleanup
            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task RemoveAsync_NonExistentEvent_ReturnsFailure()
        {
            // Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new EventRepository(context);

            // Use a random EventId that was never saved
            var fakeId = new EventId();

            // Act
            var removeResult = await repository.RemoveAsync(fakeId);

            // Assert: RemoveAsync should fail because the entity does not exist
            Assert.True(removeResult.IsFailure, "RemoveAsync should return failure for a non-existent entity.");
            Assert.Contains(
                removeResult.Errors,
                e => e.Code == "DATABASE_ERROR" && e.Message.Contains("not found")
            );

            // Cleanup
            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task GetAsync_NonExistentEvent_ReturnsFailure()
        {
            // Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new EventRepository(context);

            // Use a random EventId that was never saved
            var fakeId = new EventId();

            // Act
            var getResult = await repository.GetAsync(fakeId);

            // Assert: GetAsync should fail because the entity does not exist
            Assert.True(getResult.IsFailure, "GetAsync should return failure for a non-existent entity.");
            Assert.Contains(
                getResult.Errors,
                e => e.Code == "DATABASE_ERROR" && e.Message.Contains("not found")
            );

            // Cleanup
            await context.Database.CloseConnectionAsync();
        }
    }
}
