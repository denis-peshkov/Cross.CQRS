namespace SampleWebApp.Modules.Test;

public class TestCommandHandler : CommandHandler<TestCommand>
{
    public TestCommandHandler(ICommandEventQueueWriter commandEvents)
        : base(commandEvents)
    {
    }

    protected override Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        CommandEvents.Write(new AddNoteEvent(command.CommandId));

        CommandEvents.Write(new AddNoteOnExceptionEvent(command.CommandId));

        throw new NotImplementedException();
    }
}
