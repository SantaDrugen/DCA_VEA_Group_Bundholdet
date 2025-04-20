using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Application.Features.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Tests.Infrastructure.Repositories;
using FakeItEasy;

 namespace UnitTests
{
    public class ParticipateHandlerTests
    {
        private readonly InMemoryEventRepository  _events = new();
        private readonly InMemoryGuestRepository  _guests = new();
        private readonly IUnitOfWork              _uow    = A.Fake<IUnitOfWork>();

        private readonly VeaEvent   _activePublicEvent;
        private readonly VeaGuest   _someGuest;
        
        private VeaGuest   _aGuest;
        private VeaGuest   _bGuest;
        private VeaGuest   _cGuest;
        private VeaGuest   _dGuest;

        public ParticipateHandlerTests()
        {
            // fake uow
            A.CallTo(() => _uow.SaveChangesAsync())
             .Returns(Task.FromResult(Results.Success()));

            // seed guest
            var gEmail = EmailAddress.From("abc@via.dk").Value;
            var gFn    = FirstName.From("john").Value;
            var gLn    = LastName.From("doe").Value;
            var gUrl   = PictureUrl.From("https://pics/x.jpg").Value;
            _someGuest  = VeaGuest.Create(gEmail, gFn, gLn, gUrl).Value;
            _guests.CreateAsync(_someGuest).Wait();

            // seed event (active, public, capacity 10, future start)
            _activePublicEvent = VeaEvent.CreateNewEvent("BBQ").Value;
            _activePublicEvent.SetDescription("Yum");          // non‑default
            _activePublicEvent.SetTitle("BBQ2");                // non‑default
            _activePublicEvent.SetVisibilityPublic();
            _activePublicEvent.SetMaxGuests(5);                // 5..50 inclusive

            var dt = new EventDateTime(
                DateTime.Now.AddMinutes(10),     // must be future in LOCAL time
                DateTime.Now.AddHours(3));
            _activePublicEvent.SetDateTime(dt);

            _activePublicEvent.SetEventStatusReady();
            var activeRes = _activePublicEvent.SetEventStatusActive();
            if (activeRes.IsFailure)
                throw new Exception("Fixture error: " +
                                    string.Join(", ", activeRes.Errors.Select(e => e.Code)));

            _events.CreateAsync(_activePublicEvent).Wait();
        }

        [Fact]
        public async Task Guest_Can_Join_Public_Active_Future_Event()
        {
            var cmd  = ParticipateInPublicEventCommand
                       .Create(_activePublicEvent.Id.Value.ToString(),
                               _someGuest.Id.Value.ToString()).Value;

            var h    = new ParticipateInPublicEventHandler(_events, _guests, _uow);
            var res  = await h.HandleAsync(cmd);

            Assert.True(res.IsSuccess);
        }
        
        [Fact]
        public async Task Cannot_Join_When_Event_Not_Active()
        {
            // put event back to Draft
            _activePublicEvent.SetCancelled();          // or SetReady(), either is non‑active

            var cmd = ParticipateInPublicEventCommand
                .Create(_activePublicEvent.Id.Value.ToString(),
                    _someGuest.Id.Value.ToString()).Value;

            var h   = new ParticipateInPublicEventHandler(_events, _guests, _uow);
            var res = await h.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "EVENT_NOT_ACTIVE");
        }
        
        [Fact] // F2 – No more room
        public async Task Cannot_Join_When_Event_Is_Full()
        {
            // first slot already held by someGuest (added later in handler)
            _activePublicEvent.RegisterParticipant(_someGuest.Id, DateTime.Now);

            // fill the other 4 slots
            var letters = new[] { "aaa", "bbb", "ccc", "ddd" };
            foreach (var local in letters)
            {
                var g = VeaGuest.Create(
                    EmailAddress.From($"{local}@via.dk").Value,   // 3‑letter local part
                    FirstName.From(local).Value,
                    LastName.From(local).Value,
                    PictureUrl.From($"https://pics/{local}.jpg").Value).Value;

                await _guests.CreateAsync(g);
                _activePublicEvent.RegisterParticipant(g.Id, DateTime.Now);
            }

            // now a 6th guest tries to join via handler
            var overflow = VeaGuest.Create(
                EmailAddress.From("ovr@via.dk").Value,
                FirstName.From("over").Value,
                LastName.From("flow").Value,
                PictureUrl.From("https://pics/of.jpg").Value).Value;
            _guests.CreateAsync(overflow).Wait();

            var cmd = ParticipateInPublicEventCommand
                .Create(_activePublicEvent.Id.Value.ToString(),
                    overflow.Id.Value.ToString()).Value;

            var h   = new ParticipateInPublicEventHandler(_events, _guests, _uow);
            var res = await h.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "NO_MORE_ROOM");
        }
        
        [Fact] // F3 – Event already started
        public async Task Cannot_Join_When_Event_Has_Started()
        {
            var startUtc = DateTime.UtcNow.AddSeconds(1);   // 1s in the future

            var soon = VeaEvent.CreateNewEvent("Soon").Value;
            soon.SetDescription("Soon event");
            soon.SetVisibilityPublic();
            soon.SetMaxGuests(5);
            soon.SetDateTime(new EventDateTime(
                startUtc,
                startUtc.AddHours(2)));

            soon.SetReady();      // lightweight helpers avoid time checks
            soon.SetActive();
            await _events.CreateAsync(soon);

            // wait until we are definitely past the UTC start time (+300ms safety)
            var wait = startUtc - DateTime.UtcNow + TimeSpan.FromMilliseconds(300);
            if (wait > TimeSpan.Zero) await Task.Delay(wait);

            var cmd = ParticipateInPublicEventCommand.Create(
                soon.Id.Value.ToString(),
                _someGuest.Id.Value.ToString()).Value;

            var handler = new ParticipateInPublicEventHandler(_events, _guests, _uow);
            var res     = await handler.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "EVENT_ALREADY_STARTED");
        }



        [Fact] // F4 – Event is private
        public async Task Cannot_Join_When_Event_Is_Private()
        {
            // --- Arrange ----------------------------------------------------
            // Build a separate event that is PRIVATE and Active.
            var privEvent = VeaEvent.CreateNewEvent("Private").Value;
            privEvent.SetDescription("Hidden meetup");
            privEvent.SetVisibilityPrivate();              // ★ private
            privEvent.SetMaxGuests(5);
            privEvent.SetDateTime(new EventDateTime(
                DateTime.Now.AddMinutes(20),               // future start
                DateTime.Now.AddHours(2)));

            privEvent.SetEventStatusReady();
            var actRes = privEvent.SetEventStatusActive();
            Assert.True(actRes.IsSuccess);                 // sanity check

            await _events.CreateAsync(privEvent);           // store in repo

            var cmd = ParticipateInPublicEventCommand
                .Create(privEvent.Id.Value.ToString(),
                    _someGuest.Id.Value.ToString()).Value;

            // --- Act --------------------------------------------------------
            var handler = new ParticipateInPublicEventHandler(_events, _guests, _uow);
            var res     = await handler.HandleAsync(cmd);

            // --- Assert -----------------------------------------------------
            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "EVENT_NOT_PUBLIC");
        }
        
        [Fact] // F5 – Guest already registered
        public async Task Cannot_Join_When_Guest_Already_Participating()
        {
            // First join succeeds
            _activePublicEvent.RegisterParticipant(_someGuest.Id, DateTime.UtcNow);

            // Guest attempts to join again
            var cmd = ParticipateInPublicEventCommand
                .Create(_activePublicEvent.Id.Value.ToString(),
                    _someGuest.Id.Value.ToString()).Value;

            var handler = new ParticipateInPublicEventHandler(_events, _guests, _uow);
            var res     = await handler.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "ALREADY_REGISTERED");
        }
    }
}
