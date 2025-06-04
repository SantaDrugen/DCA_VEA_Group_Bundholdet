// File: EventAssociation.Presentation.WebAPI/EndPoints/Event/ParticipateInPublicEventEndpoint.cs

using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
using Core.Tools.ObjectMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
{
    [Route("api/[controller]")]
    public class ParticipateInPublicEventEndpoint : ApiEndpoint<ParticipateInPublicEventRequest, ParticipateInPublicEventResponse>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IObjectMapper      _mapper;

        public ParticipateInPublicEventEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper     = mapper;
        }

        [HttpPost("participate")]
        public async Task<ActionResult<ParticipateInPublicEventResponse>> Post([FromBody] ParticipateInPublicEventRequest request)
        {
            return await CustomHandle(request);
        }

        protected override async Task<Results<ParticipateInPublicEventResponse>> HandleAsync(ParticipateInPublicEventRequest request)
        {
            // 1) Build the command directly with string guestId
            var commandResult = ParticipateInPublicEventCommand.Create(request.EventId, request.GuestId);
            if (commandResult.IsFailure)
                return Results<ParticipateInPublicEventResponse>.Failure(commandResult.Errors.ToArray());

            // 2) Dispatch: handler returns Results<VeaEvent>
            var dispatchResult = await _dispatcher.DispatchAsync(commandResult.Value);
            if (dispatchResult.IsFailure)
                return Results<ParticipateInPublicEventResponse>.Failure(dispatchResult.Errors.ToArray());

            // 3) Cast to Results<VeaEvent> and unwrap
            var typedResult = (Results<VeaEvent>)dispatchResult;
            VeaEvent updatedEvent = typedResult.Value;

            // 4) Map to full EventDto
            var dto = _mapper.Map<EventDto>(updatedEvent);

            // 5) Return entire EventDto in the response
            return Results<ParticipateInPublicEventResponse>.Success(new ParticipateInPublicEventResponse
            {
                eventDto = dto
            });
        }
    }
}
