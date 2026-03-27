using Cross.CQRS.Behaviors;
using Cross.CQRS.Commands;
using Cross.CQRS.Events;
using Cross.CQRS.Filters;
using Cross.CQRS.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Cross.CQRS.Tests;

public class BaseAbstractionsTests
{
    [Fact]
    public async Task LicenseCheckBehavior_CallsNext()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<BaseAbstractionsTests>());
        var provider = services.BuildServiceProvider();
        var behavior = new LicenseCheckBehavior<SimpleRequest, string>(provider);
        var called = false;

        var result = await behavior.Handle(new SimpleRequest(), () =>
        {
            called = true;
            return Task.FromResult("ok");
        }, CancellationToken.None);

        called.Should().BeTrue();
        result.Should().Be("ok");
    }

    [Fact]
    public void CqrsRegistrationSyntax_ExposesConstructorArguments()
    {
        var services = new ServiceCollection();
        var assemblies = new List<System.Reflection.Assembly> { typeof(BaseAbstractionsTests).Assembly };
        var behaviors = new BehaviorCollection(services);

        var syntax = new CqrsRegistrationSyntax(services, assemblies, behaviors);

        syntax.Services.Should().BeSameAs(services);
        syntax.Assemblies.Should().BeSameAs(assemblies);
        syntax.Behaviors.Should().BeSameAs(behaviors);
    }

    [Fact]
    public void RequestFilter_ApplyFilter_UsesAsyncImplementation()
    {
        var filter = new TestRequestFilter();

        var result = filter.ApplyFilter(new SimpleRequest());

        result.Should().NotBeNull();
        filter.Calls.Should().Be(1);
    }

    [Fact]
    public void ResultFilter_ApplyFilter_UsesAsyncImplementation()
    {
        var filter = new TestResultFilter();

        var result = filter.ApplyFilter(10);

        result.Should().Be(11);
        filter.Calls.Should().Be(1);
    }

    [Fact]
    public async Task CommandHandler_HandleAndInterfaceHandle_CallHandleAsync()
    {
        var queue = new CommandEventQueue();
        var handler = new TestCommandHandler(queue.Writer, NullLogger<CommandHandler<TestCommand>>.Instance);
        var command = new TestCommand();

        await handler.Handle(command, CancellationToken.None);
        var iface = (IRequestHandler<TestCommand, Unit>)handler;
        var unit = await iface.Handle(command, CancellationToken.None);

        handler.Calls.Should().Be(2);
        unit.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task QueryHandler_Handle_ReturnsValue()
    {
        var handler = new TestQueryHandler(NullLogger<QueryHandler<TestQuery, int>>.Instance);

        var result = await handler.Handle(new TestQuery(), CancellationToken.None);

        result.Should().Be(42);
        handler.Calls.Should().Be(1);
    }

    [Fact]
    public async Task CommandEventHandler_Handle_CallsHandleAsync()
    {
        var handler = new TestEventHandler(NullLogger<CommandEventHandler<TestEvent>>.Instance);
        var evt = new TestEvent(Guid.NewGuid());

        await handler.Handle(evt, CancellationToken.None);

        handler.Calls.Should().Be(1);
    }

    public sealed record SimpleRequest : IRequest<string>;
    public sealed record IntRequest : IRequest<int>;

    private sealed class TestRequestFilter : RequestFilter<SimpleRequest, string>
    {
        public int Calls { get; private set; }

        public override Task<SimpleRequest> ApplyFilterAsync(SimpleRequest request, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(request);
        }
    }

    private sealed class TestResultFilter : ResultFilter<IntRequest, int>
    {
        public int Calls { get; private set; }

        public override Task<int> ApplyFilterAsync(int result, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(result + 1);
        }
    }

    private sealed class TestCommand : Command;

    private sealed class TestCommandHandler(ICommandEventQueueWriter writer, Microsoft.Extensions.Logging.ILogger<CommandHandler<TestCommand>> logger)
        : CommandHandler<TestCommand>(writer, logger)
    {
        public int Calls { get; private set; }

        protected override Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.CompletedTask;
        }
    }

    private sealed class TestQuery : Query<int>;

    private sealed class TestQueryHandler(Microsoft.Extensions.Logging.ILogger<QueryHandler<TestQuery, int>> logger)
        : QueryHandler<TestQuery, int>(logger)
    {
        public int Calls { get; private set; }

        protected override Task<int> HandleAsync(TestQuery query, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(42);
        }
    }

    private sealed class TestEvent(Guid commandId) : ICommandEvent
    {
        public Guid CommandId { get; } = commandId;
    }

    private sealed class TestEventHandler(Microsoft.Extensions.Logging.ILogger<CommandEventHandler<TestEvent>> logger)
        : CommandEventHandler<TestEvent>(logger)
    {
        public int Calls { get; private set; }

        protected override Task HandleAsync(TestEvent commandEvent, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.CompletedTask;
        }
    }
}
