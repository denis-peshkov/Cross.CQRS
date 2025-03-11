namespace SampleWebApp.Modules.TestQueryGeneric;

public class TestQueryGenericHandler : QueryHandler<TestQueryGeneric, int>
{
    public TestQueryGenericHandler(ILogger<TestQueryGenericHandler> logger)
        : base(logger)
    {
    }

    protected override Task<int> HandleAsync(TestQueryGeneric eventsCommand, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestQueryGenericHandler");

       return Task.FromResult(2);
    }
}
