namespace SampleWebApp.Modules.Test;

public class AddNoteEvent  : ICommandEvent
{
    public Guid CommandId { get; }

    public AddNoteEvent(Guid commandId)
    {
        CommandId = commandId;
    }
}
