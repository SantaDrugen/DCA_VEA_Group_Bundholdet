using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Domain.RepositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Repositories
{
    public class GuestRepository : RepositoryBase<VeaGuest, GuestId>, IGuestRepository
    {
        private readonly VeaDbContext _context;
        public GuestRepository(VeaDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Results<VeaGuest>> GetByEmailAsync(EmailAddress email)
        {
            var entity = await _context.Set<VeaGuest>()
                .FirstOrDefaultAsync(g => g.Email == email);

            if (entity is null)
            {
                return Results<VeaGuest>.Failure(
                    new Error("DATABASE_ERROR", $"Guest with email '{email.Value}' not found.")
                );
            }

            return Results<VeaGuest>.Success(entity);
        }
    }
}
