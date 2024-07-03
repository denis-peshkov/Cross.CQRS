namespace SampleWebApp.Modules.Some.Handlers;

public class SomeRequestFilter : RequestFilter<SomeQuery, IEnumerable<string>>
{
    public override Task<SomeQuery> ApplyFilterAsync(SomeQuery request, CancellationToken cancellationToken)
    {
        // do some filter actions

        return Task.FromResult(request);
    }
}
