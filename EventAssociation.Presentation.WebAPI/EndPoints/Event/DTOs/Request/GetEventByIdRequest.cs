﻿namespace EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Request;

public class GetEventByIdRequest
{
    // This isn’t bound from JSON, so we’ll pull it from the Route.
    public string EventId { get; set; } = string.Empty;
}