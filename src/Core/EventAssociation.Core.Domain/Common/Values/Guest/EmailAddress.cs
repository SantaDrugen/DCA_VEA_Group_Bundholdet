using System.Text.RegularExpressions;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Guest;

public sealed class EmailAddress
{
    private static readonly Regex ViaRegex =
        new(@"^(?<local>[A-Za-z]{3,4}|\d{6})@via\.dk$", RegexOptions.IgnoreCase);

    public string Value { get; }

    private EmailAddress(string value) => Value = value.ToLowerInvariant();

    public static Results<EmailAddress> From(string raw)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(raw))
            errors.Add(new("EMAIL_EMPTY", "E‑mail is required."));

        if (!ViaRegex.IsMatch(raw ?? string.Empty))
            errors.Add(new("EMAIL_INVALID",
                "E‑mail must be 3–4 letters OR 6 digits followed by '@via.dk'."));

        return errors.Any()
            ? Results<EmailAddress>.Failure(errors.ToArray())
            : Results<EmailAddress>.Success(new EmailAddress(raw!));
    }

    public override string ToString() => Value;
}