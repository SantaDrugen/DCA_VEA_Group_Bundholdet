using System.Diagnostics;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.Logging;

namespace EventAssociation.Core.Application.Dispatch
{
    public class LoggingDispatcher : ICommandDispatcher
    {
        private readonly ICommandDispatcher _next;
        private readonly ILogger<LoggingDispatcher> _logger;

        public LoggingDispatcher(ICommandDispatcher next, ILogger<LoggingDispatcher> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task<Results> DispatchAsync<TCommand>(TCommand command)
        {
            _logger.LogInformation("Handling {command}", typeof(TCommand).Name);
            var sw = Stopwatch.StartNew();
            var result = await _next.DispatchAsync(command);
            sw.Stop();
            _logger.LogInformation(
                "{Command} {Status} in {Elapsed}ms",
                typeof(TCommand).Name,
                result.IsSuccess ? "Succeeded" : "failed",
                sw.ElapsedMilliseconds);
            return result;
        }
    }
}
