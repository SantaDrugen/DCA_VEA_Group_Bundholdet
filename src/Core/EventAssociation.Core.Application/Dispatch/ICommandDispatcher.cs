using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Dispatch
{
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Routes a command to its handler.
        /// THrows if no handler registered.
        /// </summary>

        Task<Results> DispatchAsync<TCommand>(TCommand command);
    }
}
