using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.ReositoryInterfaces
{
    public interface IGenericRepository<T, TKey>
    {
        public Task<Results<T>> GetAsync(TKey id);
        public Task<Results> RemoveAsync(TKey id);
        public Task<Results> AddAsync(T aggregate);
    }
}
