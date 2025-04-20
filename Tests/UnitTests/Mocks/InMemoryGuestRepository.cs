using System.Collections.Concurrent;
using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common.Values.Guest;
using EventAssociation.Core.Domain.RepositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Tests.Infrastructure.Repositories;

public sealed class InMemoryGuestRepository : IGuestRepository
{
    private readonly ConcurrentDictionary<string, VeaGuest> _store = new();

    public Task<Results<VeaGuest>> CreateAsync(VeaGuest guest)
    {
        if (!_store.TryAdd(guest.Email.Value, guest))
            return Task.FromResult(
                Results<VeaGuest>.Failure(new Error("EMAIL_EXISTS", "E‑mail already registered.")));

        return Task.FromResult(Results<VeaGuest>.Success(guest));
    }

    public Task<Results<VeaGuest>> GetByEmailAsync(EmailAddress email) =>
        _store.TryGetValue(email.Value, out var g)
            ? Task.FromResult(Results<VeaGuest>.Success(g))
            : Task.FromResult(Results<VeaGuest>.Failure(
                new Error("NOT_FOUND", $"No guest with e‑mail {email}.")));
}