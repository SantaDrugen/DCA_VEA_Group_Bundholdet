using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Core.Application.Features;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Dispatcher
{
    public class CommandDispatcherTests
    {
        class TestCommand { public int X; }
        class TestHandler : ICommandHandler<TestCommand>
        {
            public bool Called;
            public Task<Results> HandleAsync(TestCommand cmd)
            {
                Called = true;
                return Task.FromResult(Results.Success());
            }
        }

        [Fact]
        public async Task Dispatch_WithRegisteredHandler_InvokesHandle()
        {
            var services = new ServiceCollection();
            services.AddScoped<ICommandHandler<TestCommand>, TestHandler>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<ICommandDispatcher>();
            var handler = provider.GetRequiredService<ICommandHandler<TestCommand>>() as TestHandler;

            var result = await dispatcher.DispatchAsync(new TestCommand { X = 42 });

            Assert.True(result.IsSuccess);
            Assert.True(handler.Called);
        }

        [Fact]
        public async Task Dispatch_WithoutHandler_ThrowsInvalidOperation()
        {
            var services = new ServiceCollection();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => dispatcher.DispatchAsync(new TestCommand()));
        }


    }
}
