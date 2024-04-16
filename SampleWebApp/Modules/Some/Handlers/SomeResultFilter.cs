namespace SampleWebApp.Modules.Some.Handlers;

public class SomeResultFilter : ResultFilter<SomeQuery, IEnumerable<string>>
{
    public override Task<IEnumerable<string>> ApplyFilterAsync(IEnumerable<string> response, CancellationToken cancellationToken)
    {
        // do some filter actions

        return Task.FromResult(response);
    }
}
