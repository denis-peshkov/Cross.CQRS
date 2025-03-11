namespace SampleWebApp.Modules.TestEventsCommand;

public class TestOnExceptionEventHandler : CommandEventHandler<TestOnExceptionEvent>
{
    public TestOnExceptionEventHandler(ILogger<TestOnExceptionEventHandler> logger)
        : base(logger)
    {
    }

    protected override async Task HandleAsync(TestOnExceptionEvent @event, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestOnExceptionEventHandler");
    }
}
