// using EventAssociation.Core.Domain.Aggregates.Event;
// using EventAssociation.Core.Domain.ReositoryInterfaces;
// using EventAssociation.Core.Tools.OperationResult;
// using EventAssociation.Presentation.WebAPI.EndPoints.Common;
// using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;
// using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;
// using Core.Tools.ObjectMapper;
// using Microsoft.AspNetCore.Mvc;
// using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
// using EventId = EventAssociation.Core.Domain.Common.Values.Event.EventId;
//
// TODO: The below code was written for a specific endpoint to get an event by ID. which is not currently implemented.
//
// namespace EventAssociation.Presentation.WebAPI.EndPoints.Event
// {
//     [Route("api/[controller]")]
//     public class GetEventByIdEndpoint : ApiEndpoint<GetEventByIdRequest, GetEventByIdResponse>
//     {
//         private readonly IEventRepository _eventRepo;
//         private readonly IObjectMapper    _mapper;
//
//         public GetEventByIdEndpoint(IEventRepository eventRepo, IObjectMapper mapper)
//         {
//             _eventRepo = eventRepo;
//             _mapper    = mapper;
//         }
//
//         [HttpGet("{eventId}")]
//         public async Task<ActionResult<GetEventByIdResponse>> Get([FromRoute] string eventId)
//         {
//             return await CustomHandle(new GetEventByIdRequest { EventId = eventId });
//         }
//
//         protected override async Task<Results<GetEventByIdResponse>> HandleAsync(GetEventByIdRequest request)
//         {
//             // 1) Wrap EventId into the VO
//             var idResult = EventId.FromString(request.EventId);
//             if (idResult.IsFailure)
//                 return Results<GetEventByIdResponse>.Failure(idResult.Errors.ToArray());
//
//             // 2) Fetch from repository
//             var fetchResult = await _eventRepo.GetByIdAsync(idResult.Value);
//             if (fetchResult.IsFailure)
//                 return Results<GetEventByIdResponse>.Failure(fetchResult.Errors.ToArray());
//
//             VeaEvent veaEvent = fetchResult.Value!;
//
//             // 3) Map to full EventDto
//             var dto = _mapper.Map<EventDto>(veaEvent);
//
//             // 4) Return entire EventDto in the response
//             return Results<GetEventByIdResponse>.Success(new GetEventByIdResponse
//             {
//                 eventDto = dto
//             });
//         }
//     }
// }
