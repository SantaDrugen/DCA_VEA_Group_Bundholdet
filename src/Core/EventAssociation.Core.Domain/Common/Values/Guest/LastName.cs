using System.Text.RegularExpressions;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Guest;

public sealed class LastName
{
    private static readonly Regex LettersOnly = new(@"^[A-Za-z]{2,25}$");

    public string Value { get; }

    private LastName(string value) => Value = Capitalize(value);

    public static Results<LastName> From(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw) || !LettersOnly.IsMatch(raw))
            return Results<LastName>.Failure(
                new Error("LASTNAME_INVALID",
                    "Last name must be 2‑25 letters (A–Z)."));

        return Results<LastName>.Success(new LastName(raw));
    }

    private static string Capitalize(string s) =>
        char.ToUpperInvariant(s[0]) + s[1..].ToLowerInvariant();

    public override string ToString() => Value;
}