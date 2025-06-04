using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Tools.OperationResult;
using Core.Tools.ObjectMapper;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
{
    [Route("api/[controller]")]
    public class CreateEventEndpoint : ApiEndpoint<CreateEventRequest, CreateEventResponse>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IObjectMapper      _mapper;

        public CreateEventEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper     = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CreateEventResponse>> Post()
        {
            return await CustomHandle(new CreateEventRequest());
        }

        protected override async Task<Results<CreateEventResponse>> HandleAsync(CreateEventRequest request)
        {
            // 1) Build the command (no payload)
            var commandResult = CreateNewEventCommand.Create();
            if (commandResult.IsFailure)
                return Results<CreateEventResponse>.Failure(commandResult.Errors.ToArray());

            // 2) Dispatch (we assume the handler returns Results<VeaEvent>)
            var dispatchResult = await _dispatcher.DispatchAsync(commandResult.Value);
            if (dispatchResult.IsFailure)
                return Results<CreateEventResponse>.Failure(dispatchResult.Errors.ToArray());

            // 3) Cast to Results<VeaEvent> and unwrap
            var typedResult = (Results<VeaEvent>)dispatchResult;
            VeaEvent createdEvent = typedResult.Value;

            // 4) Map to EventDto (requires custom mapping)
            var dto = _mapper.Map<EventDto>(createdEvent);

            // 5) Return the DTO inside our response wrapper
            return Results<CreateEventResponse>.Success(new CreateEventResponse
            {
                eventDto = dto
            });
        }
    }
}
