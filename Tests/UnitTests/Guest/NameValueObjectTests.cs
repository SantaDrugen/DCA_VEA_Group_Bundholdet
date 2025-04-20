using EventAssociation.Core.Domain.Common.Values.Guest;

namespace UnitTests.Guest;

public class NameValueObjectTests
{
    [Theory] [InlineData("john")]  [InlineData("Alice")]
    public void FirstName_Success(string raw)
    {
        var r = FirstName.From(raw);
        Assert.True(r.IsSuccess);
        Assert.Equal(char.ToUpper(raw[0]) + raw[1..].ToLower(), r.Value.Value);
    }

    [Theory] [InlineData("j")] [InlineData("A234")] [InlineData("John!")]
    public void FirstName_Failure(string raw)
    {
        var r = FirstName.From(raw);
        Assert.True(r.IsFailure);
        Assert.Contains(r.Errors, e => e.Code == "FIRSTNAME_INVALID");
    }

    [Theory] [InlineData("doe")] [InlineData("Mcgregor")]
    public void LastName_Success(string raw)
    {
        var r = LastName.From(raw);
        Assert.True(r.IsSuccess);
        Assert.Equal(char.ToUpper(raw[0]) + raw[1..].ToLower(), r.Value.Value);
    }

    [Theory] [InlineData("d")] [InlineData("Van-der")] [InlineData("Doe1")]
    public void LastName_Failure(string raw)
    {
        var r = LastName.From(raw);
        Assert.True(r.IsFailure);
        Assert.Contains(r.Errors, e => e.Code == "LASTNAME_INVALID");
    }
}