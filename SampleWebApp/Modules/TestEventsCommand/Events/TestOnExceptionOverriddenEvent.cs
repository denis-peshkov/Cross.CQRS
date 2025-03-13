namespace SampleWebApp.Modules.TestEventsCommand.Events;

public class TestOnExceptionOverriddenEvent : TestOnExceptionEvent
{
    public TestOnExceptionOverriddenEvent(Guid commandId)
        : base(commandId)
    {
    }
}
