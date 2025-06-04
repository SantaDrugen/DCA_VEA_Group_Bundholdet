using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
using Core.Tools.ObjectMapper;
using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
{
    [Route("api/[controller]")]
    public class UpdateEventTitleEndpoint : ApiEndpoint<UpdateEventTitleRequest, UpdateEventTitleResponse>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IObjectMapper      _mapper;

        public UpdateEventTitleEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper     = mapper;
        }

        [HttpPut("title")]
        public async Task<ActionResult<UpdateEventTitleResponse>> Put([FromBody] UpdateEventTitleRequest request)
        {
            return await CustomHandle(request);
        }

        protected override async Task<Results<UpdateEventTitleResponse>> HandleAsync(UpdateEventTitleRequest request)
        {
            // 1) Build the command
            var commandResult = UpdateEventTitleCommand.Create(request.EventId, request.NewTitle);
            if (commandResult.IsFailure)
                return Results<UpdateEventTitleResponse>.Failure(commandResult.Errors.ToArray());

            // 2) Dispatch; underlying handler returns Results<VeaEvent>
            var dispatchResult = await _dispatcher.DispatchAsync(commandResult.Value);
            if (dispatchResult.IsFailure)
                return Results<UpdateEventTitleResponse>.Failure(dispatchResult.Errors.ToArray());

            // 3) Cast to Results<VeaEvent> and unwrap
            var typedResult = (Results<VeaEvent>)dispatchResult;
            VeaEvent updatedEvent = typedResult.Value;

            // 4) Map to full EventDto
            var dto = _mapper.Map<EventDto>(updatedEvent);

            // 5) Return entire EventDto in the response
            return Results<UpdateEventTitleResponse>.Success(new UpdateEventTitleResponse
            {
                eventDto = dto
            });
        }
    }
}
