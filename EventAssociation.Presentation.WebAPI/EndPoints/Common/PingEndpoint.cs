using EventAssociation.Core.Tools.OperationResult;
using Microsoft.AspNetCore.Mvc;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Common;

[Route("api/[controller]")]
public class PingEndpoint : ApiEndpoint<PingRequest, PingResponse>
{
    [HttpGet]
    public async Task<ActionResult<PingResponse>> Get()
    {
        return await CustomHandle(new PingRequest());
    }
    
    protected override Task<Results<PingResponse>> HandleAsync(PingRequest request)
    {
        // Return a successful OperationResult with the Pong response
        var payload = new PingResponse
        {
            Message = "Pong! Bundholdet.WebAPI is working with OperationResult."
        };

        return Task.FromResult(Results<PingResponse>.Success(payload));
    }
}