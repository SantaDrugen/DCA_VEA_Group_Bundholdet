using EventAssociation.Core.Tools.OperationResult;
using Microsoft.AspNetCore.Mvc;

namespace EventAssociation.Presentation.WebAPI.EndPoints.Common;

// A base class for Web API endpoints that return OperationResult<TResponse>.
// If the handler returns a failure, we convert it into a 4xx/5xx automatically.
// If the handler returns a success, we return a 200 OK with the result.
[ApiController]
public abstract class ApiEndpoint<TRequest, TResponse> : ControllerBase 
    where TRequest : class 
    where TResponse : class
{
    // Implement this in each endpoint. Return an OperationResult with either
    //   - Success: the payload (TResponse),
    //   - Failure: a list of errors or a business‐rule violation, etc.
    protected abstract Task<Results<TResponse>> HandleAsync(TRequest request);
    
    // Call from your controller action. Inspects the OperationResult:
    //   • If Success, returns 200 OK + payload.
    //   • If Failure with Validation Errors, returns 400 BadRequest + error list.
    //   • If Failure with any other error, returns 409 Conflict
    protected async Task<ActionResult<TResponse>> CustomHandle(TRequest request)
    {
        var result = await HandleAsync(request);

        if (result.IsSuccess)
        {
            // 200 OK with the payload
            return Ok(result.Value);
        }

        // Failure case: choose status code based on error type. 
        // Here we assume: 
        //   • If there are validation messages, 400 Bad Request.
        //   • Otherwise, 409 Conflict
        if (result.Errors != null)
        {
            // Return all validation errors as a 400 payload
            return BadRequest(new { Errors = result.Errors });
        }

        // Non‐validation failure (business‐rule violation, etc.)
        return Conflict(new { Errors = result.Errors });
    }
}