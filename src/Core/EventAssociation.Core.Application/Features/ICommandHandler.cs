using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features
{
    internal interface ICommandHandler<TCommand>
    {
        internal Task<Results> HandleAsync(TCommand command);
    }
}
