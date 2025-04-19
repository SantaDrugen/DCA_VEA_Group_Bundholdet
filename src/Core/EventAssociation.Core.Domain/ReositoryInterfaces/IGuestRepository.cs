using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.RepositoryInterfaces;

public interface IGuestRepository
{
    Task<Results<VeaGuest>> CreateAsync(VeaGuest guest);
    Task<Results<VeaGuest>> GetByEmailAsync(EmailAddress email);
}