namespace Cross.CQRS.Filters;

/// <summary>
/// Base query/command filter.
/// </summary>
public abstract class RequestFilter<TRequest, TResult> : IRequestFilter<TRequest>
    where TRequest : IRequest<TResult>
{
    /// <inheritdoc />
    public TRequest ApplyFilter(TRequest request)
        => ApplyFilterAsync(request, CancellationToken.None).GetAwaiter().GetResult();

    /// <inheritdoc />
    public abstract Task<TRequest> ApplyFilterAsync(TRequest request, CancellationToken cancellationToken);
}
