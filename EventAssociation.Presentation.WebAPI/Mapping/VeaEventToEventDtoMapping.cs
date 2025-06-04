using Core.Tools.ObjectMapper;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;


namespace EventAssociation.Presentation.WebAPI.Mapping
{
    public class VeaEventToEventDtoMapping : IMapping<VeaEvent, EventDto>
    {
        public void Configure(VeaEvent source, EventDto destination)
        {
            // 1) Copy the ID
            destination.Id = source.Id.Value.ToString();

            // 2) Unwrap Title & Description VOs
            destination.Title = source.title?.Value ?? string.Empty;
            destination.Description = source.Description?.Value ?? string.Empty;

            // 3) Unwrap EventDateTime VO
            destination.StartDateTime = source.EventDateTime?.StartDateTime.ToString() ?? null;
            destination.EndDateTime = source.EventDateTime?.EndDateTime.ToString() ?? null;

            // 4) Unwrap Visibility & Status VOs
            destination.Visibility = source.Visibility?.Value.ToString() ?? string.Empty;
            destination.Status     = source.status?.Value.ToString()     ?? string.Empty;

            // 5) Participants VO (MaxGuests & CurrentParticipants)
            destination.MaxGuests = source.Participants?.MaxGuests.Value ?? 0;
            destination.CurrentParticipants = source.Participants?.CurrentCount ?? 0;
        }
    }
}