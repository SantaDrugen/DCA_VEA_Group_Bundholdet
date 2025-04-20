// TODO: Why was this located here? This is commented out because it breaks while Common/Values/Event/EventParticipants.cs exists.
// using EventAssociation.Core.Domain.Common.Values.Event;
// using EventAssociation.Core.Domain.Common.Values.Guest;
// using EventAssociation.Core.Tools.OperationResult;
//
// namespace EventAssociation.Core.Domain.Aggregates.Event
// {
//     public class EventParticipants
//     {
//         public NumberOfGuests MaxGuests;
//         private List<GuestId> participants;
//
//         public EventParticipants(int maxGuests)
//         {
//             MaxGuests = NumberOfGuests.FromInt(maxGuests).Value;
//             participants = new List<GuestId>();
//         }
//
//         public Results<NumberOfGuests> SetMaxGuests(int maxGuests)
//         {
//             Results<NumberOfGuests> maxGuestsResult = NumberOfGuests.FromInt(maxGuests);
//             if (maxGuestsResult.IsFailure)
//             {
//                 return maxGuestsResult;
//             }
//
//             MaxGuests = maxGuestsResult.Value;
//             return Results<NumberOfGuests>.Success(MaxGuests);
//         }
//     }
// }
