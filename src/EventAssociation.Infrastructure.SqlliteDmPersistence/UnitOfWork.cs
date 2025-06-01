using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence
{
    public class UnitOfWork(VeaDbContext context) : IUnitOfWork
    {
        private readonly VeaDbContext _context = context;
        public async Task<Results> SaveChangesAsync()
        {
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Results.Success();
            }
            else
            {
                return Results.Failure(new Error("DATABASE_ERROR", "No changes were made to the database."));
            }
        }
    }
}
