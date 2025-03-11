namespace SampleWebApp.Modules.TestCommandGeneric;

public class TestCommandGenericHandler : CommandHandler<TestCommandGeneric, int>
{
    public TestCommandGenericHandler(ICommandEventQueueWriter commandEvents, ILogger<TestCommandGenericHandler> logger)
        : base(commandEvents, logger)
    {
    }

    protected override Task<int> HandleAsync(TestCommandGeneric eventsCommand, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestCommandGenericHandler");

       return Task.FromResult(1);
    }
}
