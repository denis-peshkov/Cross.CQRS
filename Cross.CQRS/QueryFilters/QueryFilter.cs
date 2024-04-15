namespace Cross.CQRS.QueryFilters;

/// <summary>
/// Base query/command filter.
/// </summary>
public abstract class QueryFilter<TQuery, TResult> : IQueryFilter<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <inheritdoc />
    public TResult Filter(TResult response)
        => FilterAsync(response, CancellationToken.None).GetAwaiter().GetResult();

    /// <inheritdoc />
    public abstract Task<TResult> FilterAsync(TResult response, CancellationToken cancellationToken);
}
