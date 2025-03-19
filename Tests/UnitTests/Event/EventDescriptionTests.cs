using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;

namespace UnitTests.Event;

public class VeaEventDescriptionTests
{
    [Fact]
    public void SetDescription_Success_WhenEventIsDraft()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var result = veaEvent.SetDescription("Short valid description.");

        Assert.True(result.IsSuccess);
        Assert.Equal("Short valid description.", result.Value.Value);
    }

    [Fact]
    public void SetDescription_Success_WhenEventIsReady()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;
        veaEvent.SetReady();

        var result = veaEvent.SetDescription("This is a valid description.");

        Assert.True(result.IsSuccess);
        Assert.Equal("This is a valid description.", result.Value.Value);
        Assert.Equal(EventStatus.Draft, veaEvent.status);
    }

    [Fact]
    public void SetDescription_Success_WhenSettingEmptyDescription()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var result = veaEvent.SetDescription("");

        Assert.True(result.IsSuccess);
        Assert.Equal("", result.Value.Value);
    }

    [Fact]
    public void SetDescription_Failure_WhenDescriptionIsTooLong()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;
        string longDescription = new string('A', 251);

        var result = veaEvent.SetDescription(longDescription);

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "DESCRIPTION_TOO_LONG");
    }

    [Fact]
    public void SetDescription_Failure_WhenEventIsActive()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;
        veaEvent.SetActive();

        var result = veaEvent.SetDescription("Valid description.");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "EVENT_ACTIVE");
    }

    [Fact]
    public void SetDescription_Failure_WhenEventIsCancelled()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;
        veaEvent.SetCancelled();

        var result = veaEvent.SetDescription("Valid description.");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "EVENT_CANCELLED");
    }
}