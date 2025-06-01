using System.Reflection;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using UnitTests.Database.Helpers;

namespace UnitTests.Database
{
    public class EventDbTests
    {
        [Fact]
        public async Task NewVeaEvent_CanBeSavedAndLoaded_WithDefaultValues()
        {
            //Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();

            var creationResult = VeaEvent.CreateNewEvent();
            Assert.True(creationResult.IsSuccess, "Event creation should succeed");
            var newEvent = creationResult.Value;

            //Act
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            var reloaded = await context.Events.FindAsync(newEvent.Id);
            Assert.NotNull(reloaded);

            //Assert
            Assert.Equal(newEvent.Id, reloaded.Id);
            Assert.Equal(newEvent.title, reloaded.title);
            Assert.Equal(newEvent.Description, reloaded.Description);
            Assert.Equal(newEvent.EventDateTime, reloaded.EventDateTime);
            Assert.Equal(EventStatus.Draft, reloaded.status);
            Assert.Equal(EventVisibility.Private, reloaded.Visibility);
        }

        [Fact]
        public async Task BareVeaEventWithoutAnyInitialization_Throws_DbUpdateException()
        {
            //Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();
            var bareEvent = new VeaEvent(); // No initialization

            //Act
            context.Events.Add(bareEvent);

            //Assert
            await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(async () =>
            {
                await context.SaveChangesAsync();
            });
        }

        [Fact]
        public async Task SavingEventWithNullTitle_ThrowsDbUpdateException()
        {
            // Arrange
            await using var context = TestDbContextFactory.CreateInMemoryContext();

            var creationResult = VeaEvent.CreateNewEvent();
            var evt = creationResult.Value;

            // Bypass the private setter to force null title
            var titleProp = typeof(VeaEvent)
                .GetProperty(nameof(VeaEvent.title), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
            titleProp.SetValue(evt, null); // Set title to null

            // Act
            context.Events.Add(evt);

            // Assert
            await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(async () =>
            {
                await context.SaveChangesAsync();
            });
        }
    }
}
