namespace SampleWebApp.Modules.Test;

public class AddNoteEventHandler : CommandEventHandler<AddNoteEvent>
{
    protected override async Task HandleAsync(AddNoteEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine("AddNoteEventHandler");
    }
}
