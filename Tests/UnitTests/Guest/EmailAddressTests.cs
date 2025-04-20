using EventAssociation.Core.Domain.Common.Values.Guest;

namespace UnitTests.Guest;

public class EmailAddressTests
{
    [Theory]
    [InlineData("abc@via.dk")]
    [InlineData("AbCd@VIA.dk")]
    [InlineData("123456@via.dk")]
    public void Success_When_Email_Is_Valid(string email)
    {
        var r = EmailAddress.From(email);
        Assert.True(r.IsSuccess);
        Assert.Equal(email.ToLowerInvariant(), r.Value.Value);
    }

    [Theory]
    [InlineData("abc@gmail.com")]
    [InlineData("123456@hotmail.dk")]
    public void Failure_When_Domain_Is_Not_Via(string email)
    {
        var r = EmailAddress.From(email);
        Assert.True(r.IsFailure);
        Assert.Contains(r.Errors, e => e.Code == "EMAIL_INVALID");
    }

    [Theory]
    [InlineData("abcdef@via.dk")] // 6 letters – too many
    [InlineData("ab@via.dk")]     // 2 letters – too few
    [InlineData("12345@via.dk")]  // 5 digits – too few
    [InlineData("1234567@via.dk")]// 7 digits – too many
    [InlineData("a_b@via.dk")]    // invalid chars
    public void Failure_When_Local_Part_Format_Is_Wrong(string email)
    {
        var r = EmailAddress.From(email);
        Assert.True(r.IsFailure);
        Assert.Contains(r.Errors, e => e.Code == "EMAIL_INVALID");
    }
}