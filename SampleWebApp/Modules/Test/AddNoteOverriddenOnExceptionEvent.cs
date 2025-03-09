namespace SampleWebApp.Modules.Test;

public class AddNoteOverriddenOnExceptionEvent : AddNoteOnExceptionEvent
{
    public AddNoteOverriddenOnExceptionEvent(Guid commandId)
        : base(commandId)
    {
    }
}