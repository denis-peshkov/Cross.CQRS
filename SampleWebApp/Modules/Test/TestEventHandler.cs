namespace SampleWebApp.Modules.Test;

public class TestEventHandler : CommandEventHandler<TestEvent>
{
    protected override async Task HandleAsync(TestEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine("TestEventHandler");
    }
}
