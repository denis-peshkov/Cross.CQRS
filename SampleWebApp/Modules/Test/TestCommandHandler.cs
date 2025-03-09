namespace SampleWebApp.Modules.Test;

public class TestCommandHandler : CommandHandler<TestCommand>
{
    public TestCommandHandler(ICommandEventQueueWriter commandEvents)
        : base(commandEvents)
    {
    }

    protected override Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        CommandEvents.Write(new TestEvent(command.CommandId));

        CommandEvents.Write(new TestOnExceptionEvent(command.CommandId));

        CommandEvents.Write(new TestOnExceptionOverriddenEvent(command.CommandId)); // Doesn't work, required an exact handler

        throw new NotImplementedException();
    }
}
