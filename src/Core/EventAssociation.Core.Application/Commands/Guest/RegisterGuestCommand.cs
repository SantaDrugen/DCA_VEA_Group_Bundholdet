using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Guest;

public class RegisterGuestCommand
{
    public EmailAddress Email       { get; }
    public FirstName    FirstName   { get; }
    public LastName     LastName    { get; }
    public PictureUrl   PictureUrl  { get; }

    private RegisterGuestCommand(EmailAddress email,
        FirstName firstName,
        LastName lastName,
        PictureUrl pictureUrl)
    {
        Email      = email;
        FirstName  = firstName;
        LastName   = lastName;
        PictureUrl = pictureUrl;
    }

    public static Results<RegisterGuestCommand> Create(
        string email, string firstName, string lastName, string pictureUrl)
    {
        var emailRes  = EmailAddress.From(email);
        var fnRes     = FirstName.From(firstName);
        var lnRes     = LastName.From(lastName);
        var urlRes    = PictureUrl.From(pictureUrl);

        var combined = Results.Combine(emailRes, fnRes, lnRes, urlRes);
        if (combined.IsFailure)
            return Results<RegisterGuestCommand>.Failure(combined.Errors.ToArray());

        return Results<RegisterGuestCommand>.Success(
            new RegisterGuestCommand(emailRes.Value, fnRes.Value, lnRes.Value, urlRes.Value));
    }
}