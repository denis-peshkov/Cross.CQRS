namespace SampleWebApp.Modules.Test;

public class TestOnExceptionEventHandler : CommandEventHandler<TestOnExceptionEvent>
{
    protected override async Task HandleAsync(TestOnExceptionEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine("TestOnExceptionEventHandler");
    }
}
