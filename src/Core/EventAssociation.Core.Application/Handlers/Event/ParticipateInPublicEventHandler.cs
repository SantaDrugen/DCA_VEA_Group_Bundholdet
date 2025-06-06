﻿using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Application.Handlers;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Handlers.Event;

public class ParticipateInPublicEventHandler :
    ICommandHandler<ParticipateInPublicEventCommand>
{
    private readonly IEventRepository _events;
    private readonly IGuestRepository _guests;
    private readonly IUnitOfWork _uow;

    public ParticipateInPublicEventHandler(
        IEventRepository events, IGuestRepository guests, IUnitOfWork uow)
    {
        _events = events; _guests = guests; _uow = uow;
    }

    // Removed becasue we don't have guests.

    public async Task<Results> HandleAsync(ParticipateInPublicEventCommand cmd)
    {
        // load event & guest
        //var evtRes = await _events.GetByIdAsync(cmd.EventId);
        //var gstRes = await _guests.GetByIdAsync(cmd.GuestId);

        //var preCombine = Results.Combine(evtRes, gstRes);
        //if (preCombine.IsFailure) return Results.Failure(preCombine.Errors.ToArray());

        // business rule validations happen inside the aggregate
        //var regRes = evtRes.Value.RegisterParticipant(cmd.GuestId, DateTime.UtcNow);
        //if (regRes.IsFailure) return regRes;

        //await _uow.SaveChangesAsync();
        return Results.Success();
    }
}