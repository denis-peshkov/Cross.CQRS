namespace SampleWebApp.Modules.Test;

public class AddNoteOnExceptionEvent : ICommandEvent
{
    public Guid CommandId { get; }

    public virtual CommandEventFlowTypeEnum EventFlowType()
        => CommandEventFlowTypeEnum.OnExceptionalFlow;

    public AddNoteOnExceptionEvent(Guid commandId)
    {
        CommandId = commandId;
    }
}
