using EventAssociation.Core.Application.Commands.Guest;
using EventAssociation.Core.Application.Handlers.Guest;
using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;
using FakeItEasy;
using FakeItEasy.Core;
using UnitTests.Mocks;

namespace UnitTests
{
    public class GuestHandlerTests
    {
        public readonly IGuestRepository guestRepo;
        public readonly IUnitOfWork      uow;
        public readonly VeaGuest         existingGuest;

        public GuestHandlerTests()
        {
            // in‑memory repo identical in spirit to InMemoryEventRepository
            guestRepo = new InMemoryGuestRepository();

            // put one Guest in the store so we can test “email already used”
            var email  = EmailAddress.From("abc@via.dk").Value;
            var fname  = FirstName.From("john").Value;
            var lname  = LastName.From("doe").Value;
            var url    = PictureUrl.From("https://pics.example.com/p.jpg").Value;

            existingGuest = VeaGuest.Create(email, fname, lname, url).Value;
            //guestRepo.CreateAsync(existingGuest).Wait();

            // Fake UoW – same style as existing tests
            uow = A.Fake<IUnitOfWork>();
            A.CallTo(() => uow.SaveChangesAsync())
             .ReturnsLazily((IFakeObjectCall _) => Task.FromResult(Results.Success()));
        }

        [Fact]
        public async Task RegisterGuestHandler_ShouldReturnSuccess_WhenGuestIsCreated()
        {
            // Arrange
            var commandResult = RegisterGuestCommand.Create(
                "xyz@via.dk", "alice", "smith", "https://pics.example.com/x.jpg");
            var handler = new RegisterGuestHandler(guestRepo, uow);

            // Act
            var result = await handler.HandleAsync(commandResult.Value);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterGuestHandler_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var commandResult = RegisterGuestCommand.Create(
                "abc@via.dk", "bob", "stone", "https://pics.example.com/y.jpg");
            var handler = new RegisterGuestHandler(guestRepo, uow);

            // Act
            var result = await handler.HandleAsync(commandResult.Value);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}
