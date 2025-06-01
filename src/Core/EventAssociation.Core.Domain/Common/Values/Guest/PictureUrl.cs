using System.Diagnostics;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Guest;

public sealed class PictureUrl
{
    public Uri Value { get; }

    private PictureUrl(Uri uri) => Value = uri;

    public static Results<PictureUrl> From(string raw)
    {
        if (!Uri.TryCreate(raw, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            return Results<PictureUrl>.Failure(
                new Error("URL_INVALID", "Profile picture URL must be a valid absolute http/https URI."));

        return Results<PictureUrl>.Success(new PictureUrl(uri));
    }

    public static PictureUrl From(Uri value)
    {
        Debug.Assert(value != null, "value cannot be null");
        return new PictureUrl(value);
    }

    public override string ToString() => Value.ToString();
}