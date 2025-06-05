using EventAssociation.Core.Application.Handlers;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssociation.Core.Application.Dispatch
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _services;

        public CommandDispatcher(IServiceProvider serviceProvider)
            => _services = serviceProvider;

        public async Task<Results> DispatchAsync<TCommand>(TCommand command)
        {
            var handler = _services
                .GetService<ICommandHandler<TCommand>>()
                ?? throw new InvalidOperationException(
                    $"No handler registered for {typeof(TCommand).Name}");

            return await handler.HandleAsync(command);
        }
    }
}
