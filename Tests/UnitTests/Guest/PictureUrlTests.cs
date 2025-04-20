using EventAssociation.Core.Domain.Common.Values.Guest;

namespace UnitTests.Guest;

public class PictureUrlTests
{
    [Fact]
    public void Success_On_Http_Url()
    {
        var r = PictureUrl.From("https://pics.example.com/p1.jpg");
        Assert.True(r.IsSuccess);
    }

    [Theory]
    [InlineData("pics.example.com/p1.jpg")]
    [InlineData("ftp://foo.bar/img.png")]
    public void Failure_On_Invalid_Url(string url)
    {
        var r = PictureUrl.From(url);
        Assert.True(r.IsFailure);
        Assert.Contains(r.Errors, e => e.Code == "URL_INVALID");
    }
}