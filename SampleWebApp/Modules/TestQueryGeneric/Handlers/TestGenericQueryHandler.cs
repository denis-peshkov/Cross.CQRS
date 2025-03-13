namespace SampleWebApp.Modules.TestQueryGeneric.Handlers;

public class TestGenericQueryHandler : QueryHandler<TestGenericQuery, int>
{
    public TestGenericQueryHandler(ILogger<TestGenericQueryHandler> logger)
        : base(logger)
    {
    }

    protected override Task<int> HandleAsync(TestGenericQuery eventsCommand, CancellationToken cancellationToken)
    {
        Logger.LogInformation("TestQueryGenericHandler");

       return Task.FromResult(2);
    }
}
