using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Features
{
    public interface ICommandHandler<TCommand>
    {
        /// <summary>
        /// Execute the command; return Results indicating success or failure, with messages.
        /// </summary>
        
        public Task<Results> HandleAsync(TCommand command);
    }
}
