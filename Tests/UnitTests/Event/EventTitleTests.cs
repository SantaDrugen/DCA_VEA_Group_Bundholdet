using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common.Values.Event;

namespace UnitTests.Event;

public class EventTitleTests
{
    [Fact]
    public void CreateNewEvent_Success_WhenTitleIsValid()
    {
        var result = VeaEvent.CreateNewEvent("Scary Movie Night!");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void CreateNewEvent_Failure_WhenTitleIsInvalid()
    {
        var result = VeaEvent.CreateNewEvent("");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_EMPTY");
    }

    [Fact]
    public void SetTitle_Success_WhenEventIsDraftOrReady()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var result = veaEvent.SetTitle("Graduation Gala");

        Assert.True(result.IsSuccess);
        Assert.Equal("Graduation Gala", veaEvent.title.Value);
    }

    [Fact]
    public void SetTitle_Failure_WhenTitleIsEmpty()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var result = veaEvent.SetTitle("");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_EMPTY");
    }

    [Fact]
    public void SetTitle_Failure_WhenTitleIsTooShort()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var result = veaEvent.SetTitle("XY");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_INVALID_LENGTH");
    }

    [Fact]
    public void SetTitle_Failure_WhenTitleIsTooLong()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var longTitle = new string('A', 76);
        var result = veaEvent.SetTitle(longTitle);

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_INVALID_LENGTH");
    }

    [Fact]
    public void SetTitle_Failure_WhenTitleIsNull()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;

        var result = veaEvent.SetTitle(null);

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_EMPTY");
    }

    // At time of creation, no way to change EventStatus exists.
    // Once implemented, ensure these two tests are included and working.
    [Fact]
    public void SetTitle_Failure_WhenEventIsActive()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;
        veaEvent.SetActive();

        var result = veaEvent.SetTitle("Graduation Gala");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "EVENT_ACTIVE");
    }

    [Fact]
    public void SetTitle_Failure_WhenEventIsCancelled()
    {
        var veaEvent = VeaEvent.CreateNewEvent("Scary Movie Night!").Value;
        veaEvent.SetCancelled();

        var result = veaEvent.SetTitle("Graduation Gala");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "EVENT_CANCELLED");
    }

    [Fact]
    public void EventTitle_Create_Success_WhenTitleIsValid()
    {
        var result = EventTitle.Create("Valid Title");

        Assert.True(result.IsSuccess);
        Assert.Equal("Valid Title", result.Value.Value);
    }

    [Fact]
    public void EventTitle_Create_Failure_WhenTitleIsTooShort()
    {
        var result = EventTitle.Create("X");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_INVALID_LENGTH");
    }

    [Fact]
    public void EventTitle_Create_Failure_WhenTitleIsTooLong()
    {
        var longTitle = new string('A', 76);
        var result = EventTitle.Create(longTitle);

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_INVALID_LENGTH");
    }

    [Fact]
    public void EventTitle_Create_Failure_WhenTitleIsEmpty()
    {
        var result = EventTitle.Create("");

        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Code == "TITLE_EMPTY");
    }
}