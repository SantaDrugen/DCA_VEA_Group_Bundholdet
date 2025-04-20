using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event;

public class ParticipateInPublicEventCommand
{
    public EventId EventId { get; }
    public GuestId GuestId { get; }

    private ParticipateInPublicEventCommand(EventId eid, GuestId gid)
    {
        EventId = eid; GuestId = gid;
    }

    public static Results<ParticipateInPublicEventCommand> Create(
        string eventId, string guestId)
    {
        var eidRes = EventId.FromString(eventId);
        var gidRes = GuestId.FromString(guestId);

        var combined = Results.Combine(eidRes, gidRes);
        return combined.IsFailure
            ? Results<ParticipateInPublicEventCommand>.Failure(combined.Errors.ToArray())
            : Results<ParticipateInPublicEventCommand>.Success(
                new ParticipateInPublicEventCommand(eidRes.Value, gidRes.Value));
    }
}