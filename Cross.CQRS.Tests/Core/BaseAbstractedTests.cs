namespace Cross.CQRS.Tests.Core;

public class BaseAbstractionsTests
{
    [Test]
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

    [Test]
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

    [Test]
    public void RequestFilter_ApplyFilter_UsesAsyncImplementation()
    {
        var filter = new TestRequestFilter();

        var result = filter.ApplyFilter(new SimpleRequest());

        result.Should().NotBeNull();
        filter.Calls.Should().Be(1);
    }

    [Test]
    public void ResultFilter_ApplyFilter_UsesAsyncImplementation()
    {
        var filter = new TestResultFilter();

        var result = filter.ApplyFilter(10);

        result.Should().Be(11);
        filter.Calls.Should().Be(1);
    }

    [Test]
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

    [Test]
    public async Task QueryHandler_Handle_ReturnsValue()
    {
        var handler = new TestQueryHandler(NullLogger<QueryHandler<TestQuery, int>>.Instance);

        var result = await handler.Handle(new TestQuery(), CancellationToken.None);

        result.Should().Be(42);
        handler.Calls.Should().Be(1);
    }

    [Test]
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

    private sealed class TestCommand : Command
    {
    }

    private sealed class TestCommandHandler : CommandHandler<TestCommand>
    {
        public TestCommandHandler(
            ICommandEventQueueWriter writer,
            Microsoft.Extensions.Logging.ILogger<CommandHandler<TestCommand>> logger)
            : base(writer, logger)
        {
        }

        public int Calls { get; private set; }

        protected override Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.CompletedTask;
        }
    }

    private sealed class TestQuery : Query<int>
    {
    }

    private sealed class TestQueryHandler : QueryHandler<TestQuery, int>
    {
        public TestQueryHandler(Microsoft.Extensions.Logging.ILogger<QueryHandler<TestQuery, int>> logger)
            : base(logger)
        {
        }

        public int Calls { get; private set; }

        protected override Task<int> HandleAsync(TestQuery query, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(42);
        }
    }

    private sealed class TestEvent : ICommandEvent
    {
        public TestEvent(Guid commandId)
        {
            CommandId = commandId;
        }

        public Guid CommandId { get; }
    }

    private sealed class TestEventHandler : CommandEventHandler<TestEvent>
    {
        public TestEventHandler(Microsoft.Extensions.Logging.ILogger<CommandEventHandler<TestEvent>> logger)
            : base(logger)
        {
        }

        public int Calls { get; private set; }

        protected override Task HandleAsync(TestEvent commandEvent, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.CompletedTask;
        }
    }
}
