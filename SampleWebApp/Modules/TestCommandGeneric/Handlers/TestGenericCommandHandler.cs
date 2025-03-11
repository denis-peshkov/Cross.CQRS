namespace SampleWebApp.Modules.TestCommandGeneric.Handlers;

public class TestGenericCommandHandler : CommandHandler<TestGenericCommand, int>
{
    public TestGenericCommandHandler(ICommandEventQueueWriter commandEvents, ILogger<TestGenericCommandHandler> logger)
        : base(commandEvents, logger)
    {
    }

    protected override Task<int> HandleAsync(TestGenericCommand eventsGenericCommand, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestCommandGenericHandler");

       return Task.FromResult(1);
    }
}
