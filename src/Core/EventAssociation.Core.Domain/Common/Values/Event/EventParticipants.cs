using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Event;

public class EventParticipants
{
    private readonly HashSet<GuestId> _guests = new();
    public NumberOfGuests            MaxGuests { get; private set; }

    public int CurrentCount => _guests.Count;

    public EventParticipants() {}

    public EventParticipants(int maxGuests)
    {
        MaxGuests = NumberOfGuests.FromInt(maxGuests).Value;
    }

    public Results AddGuest(GuestId id)
    {
        if (_guests.Contains(id))
            return Results.Failure(new Error("ALREADY_REGISTERED",
                "Guest is already registered for this event."));

        if (CurrentCount >= MaxGuests.Value)
            return Results.Failure(new Error("NO_MORE_ROOM",
                "Maximum number of guests reached."));

        _guests.Add(id);
        return Results.Success();
    }

    internal Results<NumberOfGuests> SetMaxGuests(int newMax)
    {
        var res = NumberOfGuests.FromInt(newMax);
        if (res.IsFailure) return res;

        if (res.Value.Value < CurrentCount)
            return Results<NumberOfGuests>.Failure(
                new Error("GUESTS_OVERFLOW",
                    "New max cannot be lower than the current number of guests."));

        MaxGuests = res.Value;
        return Results<NumberOfGuests>.Success(MaxGuests);
    }
}