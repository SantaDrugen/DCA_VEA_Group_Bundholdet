using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.RepositoryInterfaces;

public interface IGuestRepository : IGenericRepository<VeaGuest, GuestId>
{
    Task<Results<VeaGuest>> GetByEmailAsync(EmailAddress email);
}