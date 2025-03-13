namespace SampleWebApp.Modules.TestEventsCommand.Events;

public class TestEventHandler : CommandEventHandler<TestEvent>
{
    public TestEventHandler(ILogger<TestEventHandler> logger)
        : base(logger)
    {
    }

    protected override async Task HandleAsync(TestEvent @event, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestEventHandler");
    }
}
