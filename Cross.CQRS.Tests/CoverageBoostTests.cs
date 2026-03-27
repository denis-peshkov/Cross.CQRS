namespace Cross.CQRS.Tests;

public class CoverageBoostTests
{
    [Test]
    public async Task CommandHandlerGeneric_Handle_AndInterfaceHandle_ReturnExpectedResult()
    {
        var queue = new CommandEventQueue();
        var logger = NullLogger<CommandHandler<TestGenericCommand, int>>.Instance;
        var handler = new TestGenericCommandHandler(queue.Writer, logger);
        var command = new TestGenericCommand();

        var directResult = await handler.Handle(command, CancellationToken.None);
        var viaInterface = (IRequestHandler<TestGenericCommand, int>)handler;
        var interfaceResult = await viaInterface.Handle(command, CancellationToken.None);

        directResult.Should().Be(77);
        interfaceResult.Should().Be(77);
        handler.Calls.Should().Be(2);
    }

    [Test]
    public void HandlerLocator_Throws_WhenRequestTypeIsNull()
    {
        var services = new ServiceCollection();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<CoverageBoostTests>());
        var provider = services.BuildServiceProvider();
        var locator = provider.GetRequiredService<IHandlerLocator>();

        var act = () => locator.FindHandlerTypeByRequest(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task RequestAndResultBehavior_CallNext_WhenNoFiltersRegistered()
    {
        var requestBehavior = new Cross.CQRS.Behaviors.RequestFilterBehavior<SimpleRequest, int>(Array.Empty<IRequestFilter<SimpleRequest>>());
        var resultBehavior = new Cross.CQRS.Behaviors.ResultFilterBehavior<SimpleRequest, int>(Array.Empty<IResultFilter<SimpleRequest, int>>());

        var requestResult = await requestBehavior.Handle(new SimpleRequest(), () => Task.FromResult(10), CancellationToken.None);
        var result = await resultBehavior.Handle(new SimpleRequest(), () => Task.FromResult(20), CancellationToken.None);

        requestResult.Should().Be(10);
        result.Should().Be(20);
    }

    [Test]
    public void LicenseAccessor_Current_ReturnsUnconfiguredLicense_WhenKeyIsMissing()
    {
        var loggerProvider = new CoverageLoggerProvider();
        var loggerFactory = LoggerFactory.Create(b => b.AddProvider(loggerProvider));
        var config = new CqrsServiceConfiguration { LicenseKey = null };
        var accessor = new LicenseAccessor(config, loggerFactory);

        var current = accessor.Current;

        current.IsConfigured.Should().BeFalse();
    }

    [Test]
    public void LicenseAccessor_LogsInvalidLicense_WhenJwtShapeIsValidButTokenInvalid()
    {
        var loggerProvider = new CoverageLoggerProvider();
        var loggerFactory = LoggerFactory.Create(b => b.AddProvider(loggerProvider));
        var config = new CqrsServiceConfiguration { LicenseKey = "a.b.c" };
        var accessor = new LicenseAccessor(config, loggerFactory);

        _ = accessor.Current;

        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("Invalid Peshkov software license key"));
    }

    [Test]
    public void LicenseValidator_LogsErrors_ForExpiredAndUnsupportedProduct()
    {
        var loggerProvider = new CoverageLoggerProvider();
        var loggerFactory = LoggerFactory.Create(b => b.AddProvider(loggerProvider));
        var validator = new LicenseValidator(loggerFactory);
        var now = DateTimeOffset.UtcNow;
        var claims = new[]
        {
            new Claim("sub_id", Guid.NewGuid().ToString()),
            new Claim("user_id", Guid.NewGuid().ToString()),
            new Claim("iat", now.ToUnixTimeSeconds().ToString()),
            new Claim("nbf", now.AddDays(-10).ToUnixTimeSeconds().ToString()),
            new Claim("exp", now.AddDays(-1).ToUnixTimeSeconds().ToString()),
            new Claim("edition", EditionEnum.Professional.ToString()),
            new Claim("type", ProductTypeEnum.Cross_CQRS_EF.ToString())
        };
        var license = new License(claims);

        validator.Validate(license);

        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("expired"));
        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("does not include Cross.CQRS"));
        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Critical && e.Message.Contains("Please visit https://peshkov.biz"));
    }

    [Test]
    public void InternalLogExtensions_CreateExpectedScope_ForCommandAndQuery()
    {
        var loggerProvider = new CoverageLoggerProvider();
        var loggerFactory = LoggerFactory.Create(b => b.AddProvider(loggerProvider));
        var logger = loggerFactory.CreateLogger("coverage");
        var command = new LargeCommand(new string('x', 300 * 1024));
        var query = new SimpleQuery();

        logger.InternalLogInformation<int>(command, "command log");
        logger.InternalLogInformation<int>(query, "query log");

        loggerProvider.Scopes.Should().HaveCountGreaterOrEqualTo(2);
        var commandScope = loggerProvider.Scopes[0].Should().BeAssignableTo<IReadOnlyCollection<KeyValuePair<string, object?>>>().Subject;
        var queryScope = loggerProvider.Scopes[1].Should().BeAssignableTo<IReadOnlyCollection<KeyValuePair<string, object?>>>().Subject;
        commandScope.Should().Contain(k => k.Key == "Command" && k.Value == null);
        commandScope.Should().Contain(k => k.Key == "CommandId" && k.Value != null);
        queryScope.Should().Contain(k => k.Key == "QueryId" && k.Value != null);
    }

    [Test]
    public void InternalLogExtensions_Work_ForCommandEventAndUnknownInternalLogObject()
    {
        var loggerProvider = new CoverageLoggerProvider();
        var loggerFactory = LoggerFactory.Create(b => b.AddProvider(loggerProvider));
        var logger = loggerFactory.CreateLogger("coverage");
        var evt = new TestCommandEvent(Guid.NewGuid());
        var unknown = new UnknownInternalLogObject("payload");

        logger.InternalLogWarning<int>(evt, new InvalidOperationException("event failed"), "event");
        logger.InternalLogError<int>(unknown, "unknown");
        logger.InternalLogCritical<int>(unknown, new InvalidOperationException("critical"), "critical");

        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Warning);
        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Error);
        loggerProvider.Entries.Should().Contain(e => e.Level == LogLevel.Critical);
        var eventScope = loggerProvider.Scopes[0].Should().BeAssignableTo<IReadOnlyCollection<KeyValuePair<string, object?>>>().Subject;
        eventScope.Should().Contain(k => k.Key == "CommandEventId" || k.Key == "CommandId");
    }

    public sealed record SimpleRequest : IRequest<int>;

    private sealed class TestGenericCommand : Command<int>
    {
    }

    private sealed class TestGenericCommandHandler : CommandHandler<TestGenericCommand, int>
    {
        public TestGenericCommandHandler(
            ICommandEventQueueWriter commandEvents,
            ILogger<CommandHandler<TestGenericCommand, int>> logger)
            : base(commandEvents, logger)
        {
        }

        public int Calls { get; private set; }

        protected override Task<int> HandleAsync(TestGenericCommand command, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(77);
        }
    }

    private sealed class LargeCommand : Command<int>
    {
        public LargeCommand(string payload)
        {
            Payload = payload;
        }

        public string Payload { get; }
    }

    private sealed class SimpleQuery : Query<int>
    {
    }

    private sealed class TestCommandEvent : ICommandEvent
    {
        public TestCommandEvent(Guid commandId)
        {
            CommandId = commandId;
        }

        public Guid CommandId { get; }
    }

    private sealed class UnknownInternalLogObject : IInternalLogObject
    {
        public UnknownInternalLogObject(string payload)
        {
            Payload = payload;
        }

        public string Payload { get; }
    }

    private sealed class CoverageLoggerProvider : ILoggerProvider
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = new();
        public List<object> Scopes { get; } = new();

        public ILogger CreateLogger(string categoryName) => new CoverageLogger(Entries, Scopes);

        public void Dispose()
        {
        }
    }

    private sealed class CoverageLogger : ILogger
    {
        private readonly List<(LogLevel Level, string Message)> _entries;
        private readonly List<object> _scopes;

        public CoverageLogger(List<(LogLevel Level, string Message)> entries, List<object> scopes)
        {
            _entries = entries;
            _scopes = scopes;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            _scopes.Add(state);
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
            => _entries.Add((logLevel, formatter(state, exception)));
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
