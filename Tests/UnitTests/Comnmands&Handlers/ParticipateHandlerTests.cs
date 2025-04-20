﻿using EventAssociation.Core.Application.Commands.Event;
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
        private readonly InMemoryEventRepository  events = new();
        private readonly InMemoryGuestRepository  guests = new();
        private readonly IUnitOfWork              uow    = A.Fake<IUnitOfWork>();

        private readonly VeaEvent   activePublicEvent;
        private readonly VeaGuest   someGuest;

        public ParticipateHandlerTests()
        {
            // fake uow
            A.CallTo(() => uow.SaveChangesAsync())
             .Returns(Task.FromResult(Results.Success()));

            // seed guest
            var gEmail = EmailAddress.From("abc@via.dk").Value;
            var gFn    = FirstName.From("john").Value;
            var gLn    = LastName.From("doe").Value;
            var gUrl   = PictureUrl.From("https://pics/x.jpg").Value;
            someGuest  = VeaGuest.Create(gEmail, gFn, gLn, gUrl).Value;
            guests.CreateAsync(someGuest).Wait();

            // seed event (active, public, capacity 10, future start)
            activePublicEvent = VeaEvent.CreateNewEvent("BBQ").Value;
            activePublicEvent.SetDescription("Yum");
            activePublicEvent.SetVisibilityPublic();
            activePublicEvent.SetMaxGuests(5);

            var dt = new EventDateTime(
                DateTime.UtcNow.AddHours(2),
                DateTime.UtcNow.AddHours(5));
            activePublicEvent.SetDateTime(dt);

            activePublicEvent.SetEventStatusReady();
            var activeRes = activePublicEvent.SetEventStatusActive();
            if (activeRes.IsFailure)
                throw new Exception("Fixture error: " +
                                    string.Join(", ", activeRes.Errors.Select(e => e.Code)));

            events.CreateAsync(activePublicEvent).Wait();
        }

        [Fact]
        public async Task Guest_Can_Join_Public_Active_Future_Event()
        {
            var cmd  = ParticipateInPublicEventCommand
                       .Create(activePublicEvent.Id.Value.ToString(),
                               someGuest.Id.Value.ToString()).Value;

            var h    = new ParticipateInPublicEventHandler(events, guests, uow);
            var res  = await h.HandleAsync(cmd);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task Cannot_Join_When_Event_Not_Active()
        {
            var cmd = ParticipateInPublicEventCommand
                      .Create(activePublicEvent.Id.Value.ToString(),
                              someGuest.Id.Value.ToString()).Value;

            var h   = new ParticipateInPublicEventHandler(events, guests, uow);
            var res = await h.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "EVENT_NOT_ACTIVE");
        }
        
        [Fact] // F2 – No more room
        public async Task Cannot_Join_When_Event_Is_Full()
        {
            // first slot already held by someGuest (added later in handler)
            activePublicEvent.RegisterParticipant(someGuest.Id, DateTime.UtcNow);

            // fill the other 4 slots
            for (int i = 0; i < 4; i++)
            {
                var extra = VeaGuest.Create(
                    EmailAddress.From($"{i}x@via.dk").Value,
                    FirstName.From("tmp").Value,
                    LastName.From("user").Value,
                    PictureUrl.From("https://pics/u.jpg").Value).Value;

                guests.CreateAsync(extra).Wait();
                activePublicEvent.RegisterParticipant(extra.Id, DateTime.UtcNow);
            }

            // now a 6th guest tries to join via handler
            var overflow = VeaGuest.Create(
                EmailAddress.From("overflow@via.dk").Value,
                FirstName.From("over").Value,
                LastName.From("flow").Value,
                PictureUrl.From("https://pics/of.jpg").Value).Value;
            guests.CreateAsync(overflow).Wait();

            var cmd = ParticipateInPublicEventCommand
                .Create(activePublicEvent.Id.Value.ToString(),
                    overflow.Id.Value.ToString()).Value;

            var h   = new ParticipateInPublicEventHandler(events, guests, uow);
            var res = await h.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "NO_MORE_ROOM");
        }


        [Fact] // F3 –Event already started
        public async Task Cannot_Join_When_Event_Has_Started()
        {
            // put the start time in the past
            var dtPast = new EventDateTime(
                           DateTime.UtcNow.AddHours(-1),
                           DateTime.UtcNow.AddHours(3));
            activePublicEvent.SetDateTime(dtPast);

            var cmd = ParticipateInPublicEventCommand
                      .Create(activePublicEvent.Id.Value.ToString(),
                              someGuest.Id.Value.ToString()).Value;

            var handler = new ParticipateInPublicEventHandler(events, guests, uow);
            var res     = await handler.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "EVENT_ALREADY_STARTED");
        }

        [Fact] // F4 –Event is private
        public async Task Cannot_Join_When_Event_Is_Private()
        {
            // make it private again
            activePublicEvent.SetVisibilityPrivate();

            var cmd = ParticipateInPublicEventCommand
                      .Create(activePublicEvent.Id.Value.ToString(),
                              someGuest.Id.Value.ToString()).Value;

            var handler = new ParticipateInPublicEventHandler(events, guests, uow);
            var res     = await handler.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "EVENT_NOT_PUBLIC");
        }
        
        [Fact] // F5 – Guest already registered
        public async Task Cannot_Join_When_Guest_Already_Participating()
        {
            // First join succeeds
            activePublicEvent.RegisterParticipant(someGuest.Id, DateTime.UtcNow);

            // Guest attempts to join again
            var cmd = ParticipateInPublicEventCommand
                .Create(activePublicEvent.Id.Value.ToString(),
                    someGuest.Id.Value.ToString()).Value;

            var handler = new ParticipateInPublicEventHandler(events, guests, uow);
            var res     = await handler.HandleAsync(cmd);

            Assert.True(res.IsFailure);
            Assert.Contains(res.Errors, e => e.Code == "ALREADY_REGISTERED");
        }
    }
}
