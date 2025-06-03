using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using Core.Tools.ObjectMapper;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Presentation.WebAPI.EndPoints.Common;
using EventAssociation.Presentation.WebAPI.EndPoints.Event;
using Microsoft.AspNetCore.Mvc;

namespace EventAssociation.Presentation.WebAPI.Endpoints.Event;

[Route("api/[controller]")]
public class CreateEventEndpoint : ApiEndpoint<CreateEventRequest, CreateEventResponse>
{
    private readonly ICommandDispatcher _dispatcher;
    private readonly IObjectMapper     _mapper;

    public CreateEventEndpoint(ICommandDispatcher dispatcher, IObjectMapper mapper)
    {
        _dispatcher = dispatcher;
        _mapper      = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CreateEventResponse>> Post()
    {
        // No [FromBody] needed because CreateEventRequest is empty
        return await CustomHandle(new CreateEventRequest());
    }

    protected override async Task<Results<CreateEventResponse>> HandleAsync(CreateEventRequest request)
    {
        // 1. Build the CreateNewEventCommand (command has no data)
        var commandResult = CreateNewEventCommand.Create();
        if (commandResult.IsFailure)
            return Results<CreateEventResponse>.Failure(commandResult.Errors.ToArray());

        // 2. Dispatch it
        var result = await _dispatcher.DispatchAsync(commandResult.Value);
        if (result.IsFailure)
            return Results<CreateEventResponse>.Failure(result.Errors.ToArray());
        
        
        var resultCast = (Results<VeaEvent>)result;
        var veaEvent = resultCast.Value;
        
        var responseDto = new CreateEventResponse
        {
            EventId = veaEvent.Id.Value.ToString()
        };

        return Results<CreateEventResponse>.Success(responseDto);
    }
}
