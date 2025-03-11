namespace SampleWebApp.Modules.TestEventsCommand;

public class TestOnExceptionOverriddenEvent : TestOnExceptionEvent
{
    public TestOnExceptionOverriddenEvent(Guid commandId)
        : base(commandId)
    {
    }
}
