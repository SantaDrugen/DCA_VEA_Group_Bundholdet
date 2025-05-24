using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Core.Application.Features;
using EventAssociation.Core.Tools.OperationResult;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace UnitTests.Dispatcher
{
    public class LoggingDispatcherTests
    {
        class DummyCommand { }

        [Fact]
        public async Task Dispatch_EmitsTwoInformationLogs_AndForwardsToInner()
        {
            // Arrange
            var cmd = new DummyCommand();
            var inner = A.Fake<ICommandDispatcher>();
            A.CallTo(() => inner.DispatchAsync(cmd))
             .Returns(Task.FromResult(Results.Success()));

            var logger = A.Fake<ILogger<LoggingDispatcher>>();
            var sut = new LoggingDispatcher(inner, logger);

            // Act
            var result = await sut.DispatchAsync(cmd);

            // Assert forwarding
            Assert.True(result.IsSuccess);
            A.CallTo(() => inner.DispatchAsync(cmd))
             .MustHaveHappenedOnceExactly();

            // Capture all calls to ILogger.Log<FormattedLogValues>
            var logCalls = Fake.GetCalls(logger)
                .Where(c => c.Method?.Name == nameof(ILogger.Log))
                .ToList();

            // Expect exactly two calls: one before, one after
            Assert.Equal(2, logCalls.Count);

            // First log (before invoking inner): should contain "Handling"
            var firstState = logCalls[0].Arguments.Get<object>(2);
            Assert.Contains("Handling", firstState.ToString());

            // Second log (after invoking inner): should indicate success
            var secondState = logCalls[1].Arguments.Get<object>(2);
            Assert.Contains("Succeeded", secondState.ToString());
        }
    }
}
