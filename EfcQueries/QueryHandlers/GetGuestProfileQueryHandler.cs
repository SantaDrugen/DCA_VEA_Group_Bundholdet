using System.Globalization;
using EfcQueries.Models;
using EfcQueries.Queries;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.QueryHandlers
{
    public class GetGuestProfileQueryHandler : IQueryHandler<GetGuestProfileQuery, GuestProfileDto>
    {
        private readonly VeadatabaseProductionContext _readContext;

        public GetGuestProfileQueryHandler(VeadatabaseProductionContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<GuestProfileDto> HandleAsync(GetGuestProfileQuery query)
        {
            var nowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            var guest = await _readContext.VeaGuests
               .Where(g => g.Id == query.GuestId)
               .Select(g => new
               {
                   g.Id,
                   g.FirstName,
                   g.LastName,
                   g.Email,
                   g.PictureUrl
               })
               .SingleOrDefaultAsync();

            if (guest is null)
                throw new InvalidOperationException($"Guest with ID {query.GuestId} not found.");

            // Base query to get all events the guest is associated with -- only asking DB once
            var baseQuery = _readContext.EventGuests
                .Include(eg => eg.Event)
                .Where(eg => eg.GuestId == query.GuestId && eg.Event!.StartDateTime != null);

            var upcomingQuery = baseQuery
               .Where(eg => eg.Event.StartDateTime.CompareTo(nowString) >= 0);

            var upcomingEventsList = await upcomingQuery
                .OrderBy(eg => eg.Event!.StartDateTime)
                .Select(eg => new EventOverviewDto(
                    eg.Event.Id,
                    eg.Event.Title,
                    eg.Event.EventGuests.Count,
                    eg.Event.StartDateTime.Substring(0, 10),
                    eg.Event.StartDateTime.Substring(11)
                ))
                .ToListAsync();

            var pastEventsList = await baseQuery
               .Where(eg => eg.Event!.StartDateTime.CompareTo(nowString) < 0)
               .OrderByDescending(eg => eg.Event.StartDateTime)
               .Take(5)
               .Select(eg => new PastEventDto(eg.Event.Title))
               .ToListAsync();

            var pendingInvitationCount = 0; // Not implemented

            return new GuestProfileDto(
                guest.Id,
                guest.FirstName,
                guest.LastName,
                guest.Email,
                guest.PictureUrl,
                upcomingEventsList.Count,
                upcomingEventsList,
                pastEventsList,
                pendingInvitationCount
            );
        }
    }
}
