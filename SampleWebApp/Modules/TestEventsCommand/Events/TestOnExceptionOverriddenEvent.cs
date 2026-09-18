namespace SampleWebApp.Modules.TestEventsCommand.Events;

public record TestOnExceptionOverriddenEvent : TestOnExceptionEvent
{
    public TestOnExceptionOverriddenEvent(Guid commandId)
        : base(commandId)
    {
    }
}
