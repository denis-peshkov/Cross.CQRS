namespace Cross.CQRS.Behaviors;

internal sealed class FilterBehavior<TQuery, TResult> : IPipelineBehavior<TQuery, TResult>
    where TQuery : IRequest<TResult>
{
    private readonly IEnumerable<IQueryFilter<TQuery, TResult>> _filters;

    public FilterBehavior(IEnumerable<IQueryFilter<TQuery, TResult>> filters)
    {
        _filters = filters;
    }

    /// <inheritdoc />
    public async Task<TResult> Handle(TQuery request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var filters = _filters.ToArray();
        if (filters.Length <= 0)
        {
            return await next();
        }

        var responce = await next();

        var result = responce;
        foreach (var filter in filters)
        {
            result = await filter.FilterAsync(result, cancellationToken);
        }

        return result;
    }
}
