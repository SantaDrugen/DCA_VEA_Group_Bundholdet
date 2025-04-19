using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guest;

public class VeaGuest
{
    public GuestId     Id           { get; }
    public EmailAddress Email       { get; private set; }
    public FirstName   FirstName    { get; private set; }
    public LastName    LastName     { get; private set; }
    public PictureUrl  PictureUrl   { get; private set; }

    private VeaGuest(GuestId id,
        EmailAddress email,
        FirstName firstName,
        LastName lastName,
        PictureUrl pictureUrl)
    {
        Id          = id;
        Email       = email;
        FirstName   = firstName;
        LastName    = lastName;
        PictureUrl  = pictureUrl;
    }

    public static Results<VeaGuest> Create(EmailAddress email,
        FirstName firstName,
        LastName lastName,
        PictureUrl pictureUrl)
    {
        // all value object validations already ran, so always succeeds.
        var guest = new VeaGuest(new GuestId(), email, firstName, lastName, pictureUrl);
        return Results<VeaGuest>.Success(guest);
    }
}