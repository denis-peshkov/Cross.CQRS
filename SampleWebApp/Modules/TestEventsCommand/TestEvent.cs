namespace SampleWebApp.Modules.TestEventsCommand;

public class TestEvent  : ICommandEvent
{
    public Guid CommandId { get; }

    public TestEvent(Guid commandId)
    {
        CommandId = commandId;
    }
}
