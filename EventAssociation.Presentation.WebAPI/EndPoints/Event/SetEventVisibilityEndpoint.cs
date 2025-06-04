using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
using Core.Tools.ObjectMapper;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using Microsoft.AspNetCore.Mvc;
using Results = EventAssociation.Core.Tools.OperationResult.Results;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
{
    [Route("api/[controller]")]
    public class SetEventVisibilityEndpoint : ApiEndpoint<SetEventVisibilityRequest, SetEventVisibilityResponse>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IObjectMapper      _mapper;

        public SetEventVisibilityEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper     = mapper;
        }

        [HttpPut("visibility")]
        public async Task<ActionResult<SetEventVisibilityResponse>> Put([FromBody] SetEventVisibilityRequest request)
        {
            return await CustomHandle(request);
        }

        protected override async Task<Results<SetEventVisibilityResponse>> HandleAsync(SetEventVisibilityRequest request)
        {
            Results dispatchResult;

            if (request.MakePrivate)
            {
                // 1A) Build “Set Private” command
                var cmdResult = SetEventPrivateCommand.Create(request.EventId);
                if (cmdResult.IsFailure)
                    return Results<SetEventVisibilityResponse>.Failure(cmdResult.Errors.ToArray());

                // 1B) Dispatch
                dispatchResult = await _dispatcher.DispatchAsync(cmdResult.Value);
            }
            else
            {
                // 2A) Build “Set Public” command
                var cmdResult = SetEventPublicCommand.Create(request.EventId);
                if (cmdResult.IsFailure)
                    return Results<SetEventVisibilityResponse>.Failure(cmdResult.Errors.ToArray());

                // 2B) Dispatch
                dispatchResult = await _dispatcher.DispatchAsync(cmdResult.Value);
            }

            if (dispatchResult.IsFailure)
                return Results<SetEventVisibilityResponse>.Failure(dispatchResult.Errors.ToArray());

            // 3) Cast to Results<VeaEvent> and unwrap
            var typedResult = (Results<VeaEvent>)dispatchResult;
            VeaEvent updatedEvent = typedResult.Value;

            // 4) Map to full EventDto
            var dto = _mapper.Map<EventDto>(updatedEvent);

            // 5) Return entire EventDto in the response
            return Results<SetEventVisibilityResponse>.Success(new SetEventVisibilityResponse
            {
                eventDto = dto
            });
        }
    }
}
