namespace SampleWebApp.Modules.TestEventsCommand.Events;

public record TestEvent : CommandEvent
{
    public TestEvent(Guid commandId)
        : base(commandId)
    {
    }
}
