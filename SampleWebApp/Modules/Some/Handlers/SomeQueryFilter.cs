namespace SampleWebApp.Modules.Some.Handlers;

public class SomeQueryFilter : QueryFilter<SomeQuery, IEnumerable<string>>
{
    public override Task<IEnumerable<string>> FilterAsync(IEnumerable<string> response, CancellationToken cancellationToken)
    {
        // do some filter actions

        return Task.FromResult(response);
    }
}
