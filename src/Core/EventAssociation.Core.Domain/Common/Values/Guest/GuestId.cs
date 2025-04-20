using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Guest;

public sealed class GuestId
{
    public Guid Value { get; }

    private GuestId(Guid id) => Value = id;

    public GuestId() : this(Guid.NewGuid()) { }

    public static Results<GuestId> FromString(string id)
    {
        if (Guid.TryParse(id, out var guid))
            return Results<GuestId>.Success(new GuestId(guid));

        return Results<GuestId>.Failure(
            new Error("GUEST_ID_INVALID", "Guest Id must be a valid GUID."));
    }

    public override bool Equals(object? obj) => obj is GuestId other && Value.Equals(other.Value);
    public override int GetHashCode() => Value.GetHashCode();
}