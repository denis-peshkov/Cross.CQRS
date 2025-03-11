namespace SampleWebApp.Modules.TestEventsCommand;

public class TestOnExceptionEvent : ICommandEvent
{
    public Guid CommandId { get; }

    public virtual CommandEventFlowTypeEnum EventFlowType()
        => CommandEventFlowTypeEnum.ExceptionSafeFlow;

    public TestOnExceptionEvent(Guid commandId)
    {
        CommandId = commandId;
    }
}
