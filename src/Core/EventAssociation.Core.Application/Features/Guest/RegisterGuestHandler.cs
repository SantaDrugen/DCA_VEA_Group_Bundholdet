using EventAssociation.Core.Application.Commands.Guest;
using EventAssociation.Core.Application.Features;               // ICommandHandler
using EventAssociation.Core.Domain.Aggregates.Guest;
using EventAssociation.Core.Domain.Common;                      // IUnitOfWork
using EventAssociation.Core.Domain.RepositoryInterfaces;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features.Guest;

public class RegisterGuestHandler : ICommandHandler<RegisterGuestCommand>
{
    private readonly IGuestRepository _repo;
    private readonly IUnitOfWork      _uow;

    public RegisterGuestHandler(IGuestRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<Results> HandleAsync(RegisterGuestCommand cmd)
    {
        var errors = new List<Error>();

        // F5 – duplicate e‑mail?
        if ((await _repo.GetByEmailAsync(cmd.Email)).IsSuccess)
            errors.Add(new("EMAIL_EXISTS", "This e‑mail is already registered."));

        // build aggregate
        var guestRes = VeaGuest.Create(cmd.Email, cmd.FirstName, cmd.LastName, cmd.PictureUrl);
        if (guestRes.IsFailure) errors.AddRange(guestRes.Errors);

        // persist
        if (!errors.Any())
        {
            //var repoRes = await _repo.CreateAsync(guestRes.Value);
            //if (repoRes.IsFailure) errors.AddRange(repoRes.Errors);
        }

        if (errors.Any()) return Results.Failure(errors.ToArray());

        await _uow.SaveChangesAsync();
        return Results.Success();
    }
}