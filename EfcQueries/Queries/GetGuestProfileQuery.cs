using QueryContracts.Contracts;
using QueryContracts.DTOs;

namespace EfcQueries.Queries
{
    public record GetGuestProfileQuery(
        
        string GuestId
        ) : IQuery<GuestProfileDto>;
}
