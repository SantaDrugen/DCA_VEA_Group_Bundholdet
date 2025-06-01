using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Repositories
{
    public abstract class RepositoryBase<T, TKey> : IGenericRepository<T, TKey>
        where T : class
    {
        protected readonly VeaDbContext Context;

        protected RepositoryBase(VeaDbContext context)
        {
            Context = context;
        }

        public async Task<Results> AddAsync(T aggregate)
        {
            var entry = await Context.Set<T>().AddAsync(aggregate);
            return entry.State == EntityState.Added
               ? Results.Success()
               : Results.Failure(new Error("DATABASE_ERROR", "Failed to add the entity."));
        }

        public virtual async Task<Results<T>> GetAsync(TKey id)
        {
            var entity = await Context.Set<T>().FindAsync(id);
            if (entity is null)
                return Results<T>.Failure(
                    new Error("DATABASE_ERROR", $"Entity with Id '{id}' not found.")
                );
            return Results<T>.Success(entity);
        }

        public async Task<Results> RemoveAsync(TKey id)
        {
            var existing = await GetAsync(id);
            if (existing.IsFailure)
                return Results.Failure(existing.Errors);

            Context.Set<T>().Remove(existing.Value!);
            return Results.Success();
        }
    }
}
