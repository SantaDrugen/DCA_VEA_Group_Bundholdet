using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common.Values.Guest;

namespace UnitTests.Guest;

public class VeaGuestTests
{
    [Fact] // S1
    public void Create_Succeeds_With_Valid_Values()
    {
        var email  = EmailAddress.From("abc@via.dk").Value;
        var fn     = FirstName.From("john").Value;
        var ln     = LastName.From("doe").Value;
        var url    = PictureUrl.From("https://x/y.jpg").Value;

        var res = VeaGuest.Create(email, fn, ln, url);

        Assert.True(res.IsSuccess);
        var g = res.Value;
        Assert.Equal("John", g.FirstName.Value);
        Assert.Equal("Doe",  g.LastName.Value);
        Assert.Equal("abc@via.dk", g.Email.Value);
    }

    // F3, F4, F6, F7 all surface through the value objects,
    // so a single test per path is enough:
    [Fact]  // F3
    public void Create_Fails_With_Bad_FirstName()
    {
        var res = FirstName.From("1x");
        Assert.True(res.IsFailure);
    }
}