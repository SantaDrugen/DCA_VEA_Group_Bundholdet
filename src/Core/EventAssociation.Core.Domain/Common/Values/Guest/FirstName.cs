using System.Text.RegularExpressions;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Guest;

public sealed class FirstName
{
    private static readonly Regex LettersOnly = new(@"^[A-Za-z]{2,25}$");

    public string Value { get; }

    private FirstName(string value) => Value = Capitalize(value);

    public static Results<FirstName> From(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw) || !LettersOnly.IsMatch(raw))
            return Results<FirstName>.Failure(
                new Error("FIRSTNAME_INVALID",
                    "First name must be 2‑25 letters (A–Z)."));

        return Results<FirstName>.Success(new FirstName(raw));
    }

    private static string Capitalize(string s) =>
        char.ToUpperInvariant(s[0]) + s[1..].ToLowerInvariant();

    public override string ToString() => Value;
}