using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
using Core.Tools.ObjectMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using Results = EventAssociation.Core.Tools.OperationResult.Results;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
{
    [Route("api/[controller]")]
    public class SetEventStatusEndpoint : ApiEndpoint<SetEventStatusRequest, SetEventStatusResponse>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IObjectMapper      _mapper;

        public SetEventStatusEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper     = mapper;
        }

        [HttpPut("status")]
        public async Task<ActionResult<SetEventStatusResponse>> Put([FromBody] SetEventStatusRequest request)
        {
            return await CustomHandle(request);
        }

        protected override async Task<Results<SetEventStatusResponse>> HandleAsync(SetEventStatusRequest request)
        {
            Results dispatchResult;

            if (string.Equals(request.NewStatus, "Ready", StringComparison.OrdinalIgnoreCase))
            {
                // 1A) Build “Set Status → Ready” command
                var cmdResult = SetEventStatusReadyCommand.Create(request.EventId);
                if (cmdResult.IsFailure)
                    return Results<SetEventStatusResponse>.Failure(cmdResult.Errors.ToArray());

                dispatchResult = await _dispatcher.DispatchAsync(cmdResult.Value);
            }
            else if (string.Equals(request.NewStatus, "Active", StringComparison.OrdinalIgnoreCase))
            {
                // 2A) Build “Set Status → Active” command
                var cmdResult = SetEventStatusActiveCommand.Create(request.EventId);
                if (cmdResult.IsFailure)
                    return Results<SetEventStatusResponse>.Failure(cmdResult.Errors.ToArray());

                dispatchResult = await _dispatcher.DispatchAsync(cmdResult.Value);
            }
            else
            {
                return Results<SetEventStatusResponse>.Failure(new[]
                {
                    new Error("INVALID_STATUS", "Status must be either 'Ready' or 'Active'.")
                });
            }

            if (dispatchResult.IsFailure)
                return Results<SetEventStatusResponse>.Failure(dispatchResult.Errors.ToArray());

            // 3) Cast to Results<VeaEvent> and unwrap
            var typedResult = (Results<VeaEvent>)dispatchResult;
            VeaEvent updatedEvent = typedResult.Value;

            // 4) Map to full EventDto
            var dto = _mapper.Map<EventDto>(updatedEvent);

            // 5) Return entire EventDto in the response
            return Results<SetEventStatusResponse>.Success(new SetEventStatusResponse
            {
                eventDto = dto
            });
        }
    }
}
