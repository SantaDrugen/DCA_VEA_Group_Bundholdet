﻿using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Application.Handlers.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;
using FakeItEasy;
using FakeItEasy.Core;
using UnitTests.Mocks;

namespace UnitTests
{
    public class HandlerTests
    {
        public readonly IEventRepository eventRepo;
        public readonly IUnitOfWork uow;

        public readonly VeaEvent createdEvent;

        public HandlerTests()
        {
            eventRepo = new InMemoryEventRepository();

            createdEvent = VeaEvent.CreateNewEvent().Value;

            eventRepo.AddAsync(createdEvent);

            uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.SaveChangesAsync())
                .ReturnsLazily((IFakeObjectCall _) =>
                {
                    return Task.FromResult(Results.Success());
                });
        }

        [Fact]
        public async Task CreateNewEventHandler_ShouldReturnSuccess_WhenEventIsCreated()
        {
            // Arrange
            var commandResult = CreateNewEventCommand.Create();
            var handler = new CreateNewEventHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateEventTitleHandler_ShouldReturnSuccess_WhenEventTitleIsUpdated()
        {
            // Arrange
            var commandResult = UpdateEventTitleCommand.Create(createdEvent.Id.Value.ToString(), "New Title");
            var handler = new UpdateEventTitleHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Title", createdEvent.title.Value);
        }

        [Fact]
        public async Task UpdateEventTitleHandler_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var commandResult = UpdateEventTitleCommand.Create(Guid.NewGuid().ToString(), "New Title");
            var handler = new UpdateEventTitleHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task UpdateEventDescriptionHandler_ShouldReturnSuccess_WhenEventDescriptionIsUpdated()
        {
            // Arrange
            var commandResult = UpdateEventDescriptionCommand.Create(createdEvent.Id.Value.ToString(), "New Description");
            var handler = new UpdateEventDescriptionHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Description", createdEvent.Description.Value);
        }

        [Fact]
        public async Task UpdateEventDescriptionHandler_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var commandResult = UpdateEventDescriptionCommand.Create(Guid.NewGuid().ToString(), "New Description");
            var handler = new UpdateEventDescriptionHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task UpdateEventDateTimeHandler_ShouldReturnSuccess_WhenEventDateTimeIsUpdated()
        {
            // Arrange
            DateTime newStartDateTime = DateTime.Now.AddDays(1);
            DateTime newEndDateTime = DateTime.Now.AddDays(1).AddHours(2);

            var commandResult = UpdateEventDateTimeCommand.Create(createdEvent.Id.Value.ToString(), newStartDateTime, newEndDateTime);
            var handler = new UpdateEventDateTimeHandler(eventRepo, uow);

            // Act
            var result = await handler.HandleAsync(commandResult.Value);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateEventDateTimeHandler_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            DateTime newStartDateTime = DateTime.Now.AddDays(1);
            DateTime newEndDateTime = DateTime.Now.AddDays(1).AddHours(2);
            var commandResult = UpdateEventDateTimeCommand.Create(Guid.NewGuid().ToString(), newStartDateTime, newEndDateTime);
            var handler = new UpdateEventDateTimeHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task SetEventPublicHandler_ShouldReturnSuccess_WhenEventIsSetPublic()
        {
            // Arrange
            var commandResult = SetEventPublicCommand.Create(createdEvent.Id.Value.ToString());
            var handler = new SetEventPublicHandler(eventRepo, uow);

            // Act
            var result = await handler.HandleAsync(commandResult.Value);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SetEventPublicHandler_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var commandResult = SetEventPublicCommand.Create(Guid.NewGuid().ToString());
            var handler = new SetEventPublicHandler(eventRepo, uow);

            // Act
            var result = await handler.HandleAsync(commandResult.Value);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task SetEventPrivateHandler_ShouldReturnSuccess_WhenEventIsSetPrivate()
        {
            // Arrange
            var commandResult = SetEventPrivateCommand.Create(createdEvent.Id.Value.ToString());
            var handler = new SetEventPrivateHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SetEventPrivateHandler_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var commandResult = SetEventPrivateCommand.Create(Guid.NewGuid().ToString());
            var handler = new SetEventPrivateHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task SetEventMaxNumberOfGuestsHandler_ShouldReturnSuccess_WhenMaxNumberOfGuestsIsSet()
        {
            // Arrange
            var commandResult = SetEventMaxNumberOfGuestsCommand.Create(createdEvent.Id.Value.ToString(), 25);
            var handler = new SetEventMaxNumberOfGuestsHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SetEventMaxNumberOfGuestsHandler_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var commandResult = SetEventMaxNumberOfGuestsCommand.Create(Guid.NewGuid().ToString(), 25);
            var handler = new SetEventMaxNumberOfGuestsHandler(eventRepo, uow);
            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task SetEventStatusReadyHandler__ShouldReturnSuccess_WhenEventStatusIsSetReady()
        {
            // Arrange
            var commandResult = SetEventStatusReadyCommand.Create(createdEvent.Id.Value.ToString());

            createdEvent.SetDescription("A valid description");
            createdEvent.SetTitle("A valid title");

            EventDateTime eventDateTime = EventDateTime.Create(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(2)).Value;

            createdEvent.SetDateTime(eventDateTime);
            createdEvent.SetVisibilityPublic();


            var handler = new SetEventStatusReadyHandler(eventRepo, uow);


            // Act
            var result = await handler.HandleAsync(commandResult.Value);
            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
