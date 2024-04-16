namespace Cross.CQRS.Filters;

/// <summary>
/// Base query/command filter.
/// </summary>
public abstract class ResultFilter<TRequest, TResult> : IResultFilter<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    /// <inheritdoc />
    public TResult ApplyFilter(TResult result)
        => ApplyFilterAsync(result, CancellationToken.None).GetAwaiter().GetResult();

    /// <inheritdoc />
    public abstract Task<TResult> ApplyFilterAsync(TResult result, CancellationToken cancellationToken);
}
