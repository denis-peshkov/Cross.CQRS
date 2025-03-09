namespace SampleWebApp.Modules.Test;

public class TestOnExceptionEvent : ICommandEvent
{
    public Guid CommandId { get; }

    public virtual CommandEventFlowTypeEnum EventFlowType()
        => CommandEventFlowTypeEnum.OnExceptionalFlow;

    public TestOnExceptionEvent(Guid commandId)
    {
        CommandId = commandId;
    }
}
