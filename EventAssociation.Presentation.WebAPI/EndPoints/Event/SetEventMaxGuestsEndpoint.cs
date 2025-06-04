using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
using Core.Tools.ObjectMapper;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
{
    [Route("api/[controller]")]
    public class SetEventMaxGuestsEndpoint : ApiEndpoint<SetEventMaxGuestsRequest, SetEventMaxGuestResponse>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IObjectMapper      _mapper;

        public SetEventMaxGuestsEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper     = mapper;
        }

        [HttpPut("maxguests")]
        public async Task<ActionResult<SetEventMaxGuestResponse>> Put([FromBody] SetEventMaxGuestsRequest request)
        {
            return await CustomHandle(request);
        }

        protected override async Task<Results<SetEventMaxGuestResponse>> HandleAsync(SetEventMaxGuestsRequest request)
        {
            // 1) Build the command directly with eventId and maxGuests
            var commandResult = SetEventMaxNumberOfGuestsCommand.Create(request.EventId, request.MaxGuests);
            if (commandResult.IsFailure)
                return Results<SetEventMaxGuestResponse>.Failure(commandResult.Errors.ToArray());

            // 2) Dispatch; handler returns Results<VeaEvent>
            var dispatchResult = await _dispatcher.DispatchAsync(commandResult.Value);
            if (dispatchResult.IsFailure)
                return Results<SetEventMaxGuestResponse>.Failure(dispatchResult.Errors.ToArray());

            // 3) Cast to Results<VeaEvent> and unwrap
            var typedResult = (Results<VeaEvent>)dispatchResult;
            VeaEvent updatedEvent = typedResult.Value;

            // 4) Map to full EventDto
            var dto = _mapper.Map<EventDto>(updatedEvent);

            // 5) Return entire EventDto in the response
            return Results<SetEventMaxGuestResponse>.Success(new SetEventMaxGuestResponse
            {
                eventDto = dto
            });
        }
    }
}
