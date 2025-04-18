using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common
{
    public interface IUnitOfWork
    {
        Task<Results<bool>> SaveChangesAsync();
    }
}
