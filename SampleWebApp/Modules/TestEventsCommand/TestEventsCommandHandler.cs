namespace SampleWebApp.Modules.TestEventsCommand;

public class TestEventsCommandHandler : CommandHandler<TestEventsCommand>
{
    public TestEventsCommandHandler(ICommandEventQueueWriter commandEvents, ILogger<TestEventsCommandHandler> logger)
        : base(commandEvents, logger)
    {
    }

    protected override Task HandleAsync(TestEventsCommand eventsCommand, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestCommandHandler Begin");

        CommandEvents.Write(new TestEvent(eventsCommand.CommandId));

        CommandEvents.Write(new TestOnExceptionEvent(eventsCommand.CommandId));

        CommandEvents.Write(new TestOnExceptionOverriddenEvent(eventsCommand.CommandId)); // Doesn't work, required an exact handler

        Logger.LogInformation("TestCommandHandler End");

        throw new NotImplementedException();
    }
}
