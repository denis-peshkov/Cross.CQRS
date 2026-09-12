namespace Cross.CQRS.Tests.Behaviors;

public class BehaviorPipelineTests
{
    [Test]
    public async Task ValidationBehavior_CallsNext_WhenNoValidators()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(Array.Empty<IValidator<TestRequest>>());
        var called = false;

        var result = await behavior.Handle(new TestRequest("ok"), () =>
        {
            called = true;
            return Task.FromResult("done");
        }, CancellationToken.None);

        called.Should().BeTrue();
        result.Should().Be("done");
    }

    [Test]
    public async Task ValidationBehavior_Throws_WhenValidationFails()
    {
        var validator = new TestRequestValidator();
        var behavior = new ValidationBehavior<TestRequest, string>(new[] { validator });

        var act = () => behavior.Handle(new TestRequest(""), () => Task.FromResult("x"), CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task RequestFilterBehavior_AppliesAllFilters_ThenCallsNext()
    {
        var trace = new List<string>();
        var filters = new IRequestFilter<TestRequest>[]
        {
            new TraceRequestFilter("f1", trace),
            new TraceRequestFilter("f2", trace)
        };
        var behavior = new RequestFilterBehavior<TestRequest, string>(filters);

        var result = await behavior.Handle(new TestRequest("ok"), () =>
        {
            trace.Add("next");
            return Task.FromResult("done");
        }, CancellationToken.None);

        result.Should().Be("done");
        trace.Should().Equal("f1", "f2", "next");
    }

    [Test]
    public async Task ResultFilterBehavior_AppliesAllFilters_InOrder()
    {
        var filters = new IResultFilter<IntRequest, int>[]
        {
            new AddResultFilter(2),
            new AddResultFilter(3)
        };
        var behavior = new ResultFilterBehavior<IntRequest, int>(filters);

        var result = await behavior.Handle(new IntRequest(), () => Task.FromResult(1), CancellationToken.None);

        result.Should().Be(6);
    }

    [Test]
    public async Task CommandEventQueueProcessBehavior_PublishesStandardAndExceptionSafeEvents()
    {
        var queue = new CommandEventQueue();
        var mediator = new FakeMediator();
        var logger = NullLogger<CommandEventQueueProcessBehavior<TestCommand, string>>.Instance;
        var behavior = new CommandEventQueueProcessBehavior<TestCommand, string>(queue.Reader, mediator, logger);
        var command = new TestCommand();

        queue.Writer.Write(new TestCommandEvent(command.CommandId, CommandEventFlowTypeEnum.StandardFlow));
        queue.Writer.Write(new TestCommandEvent(command.CommandId, CommandEventFlowTypeEnum.ExceptionSafeFlow));

        var result = await behavior.Handle(command, () => Task.FromResult("ok"), CancellationToken.None);

        result.Should().Be("ok");
        mediator.Published.Count.Should().Be(2);
    }

    [Test]
    public async Task CommandEventQueueProcessBehavior_Continues_WhenPublishThrows()
    {
        var queue = new CommandEventQueue();
        var mediator = new FakeMediator { ThrowOnPublish = true };
        var logger = NullLogger<CommandEventQueueProcessBehavior<TestCommand, string>>.Instance;
        var behavior = new CommandEventQueueProcessBehavior<TestCommand, string>(queue.Reader, mediator, logger);
        var command = new TestCommand();

        queue.Writer.Write(new TestCommandEvent(command.CommandId, CommandEventFlowTypeEnum.StandardFlow));

        var act = () => behavior.Handle(command, () => Task.FromResult("ok"), CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public void CheckLicense_Throws_WhenLoggerFactoryNotRegistered()
    {
        var services = new ServiceCollection();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<TestRequest>());
        var provider = services.BuildServiceProvider();

        var act = () => provider.CheckLicense();
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void CheckLicense_LeavesLicenseCheckedFalse_AfterCall_WhenLoggingRegistered()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<TestRequest>());
        var provider = services.BuildServiceProvider();

        LicenseCheckExtensions.ResetLicenseCheckForTests();
        provider.CheckLicense();

        // flag stays false after each check (same as Identity) — next call can validate again
        var act = () => provider.CheckLicense();
        act.Should().NotThrow();
    }

    public sealed record TestRequest(string Value) : IRequest<string>;
    public sealed record IntRequest : IRequest<int>;

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }

    private sealed class TraceRequestFilter : IRequestFilter<TestRequest>
    {
        private readonly string _marker;
        private readonly List<string> _trace;

        public TraceRequestFilter(string marker, List<string> trace)
        {
            _marker = marker;
            _trace = trace;
        }

        public TestRequest ApplyFilter(TestRequest result)
        {
            _trace.Add(_marker);
            return result;
        }

        public Task<TestRequest> ApplyFilterAsync(TestRequest result, CancellationToken cancellationToken)
        {
            _trace.Add(_marker);
            return Task.FromResult(result);
        }
    }

    private sealed class AddResultFilter : IResultFilter<IntRequest, int>
    {
        private readonly int _value;

        public AddResultFilter(int value)
        {
            _value = value;
        }

        public int ApplyFilter(int result) => result + _value;
        public Task<int> ApplyFilterAsync(int result, CancellationToken cancellationToken) => Task.FromResult(result + _value);
    }

    private sealed class TestCommand : Command<string>
    {
    }

    private sealed class TestCommandEvent : ICommandEvent
    {
        private readonly CommandEventFlowTypeEnum _flowType;

        public TestCommandEvent(Guid commandId, CommandEventFlowTypeEnum flowType)
        {
            CommandId = commandId;
            _flowType = flowType;
        }

        public Guid CommandId { get; }
        public CommandEventFlowTypeEnum EventFlowType() => _flowType;
    }

    private sealed class FakeMediator : IMediator
    {
        public bool ThrowOnPublish { get; set; }
        public List<INotification> Published { get; } = new();

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            if (notification is INotification n)
            {
                Published.Add(n);
            }

            if (ThrowOnPublish)
            {
                throw new InvalidOperationException("publish failed");
            }

            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
            => Publish((object)notification, cancellationToken);

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();
    }
}
