namespace SampleWebApp.Modules.Test;

public class TestOnExceptionOverriddenEvent : TestOnExceptionEvent
{
    public TestOnExceptionOverriddenEvent(Guid commandId)
        : base(commandId)
    {
    }
}
