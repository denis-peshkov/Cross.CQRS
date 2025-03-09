namespace SampleWebApp.Modules.Test;

public class AddNoteOnExceptionEventHandler : CommandEventHandler<AddNoteOnExceptionEvent>
{
    protected override async Task HandleAsync(AddNoteOnExceptionEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine("AddNoteOnExceptionEventHandler");
    }
}
