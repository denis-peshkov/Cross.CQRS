namespace Cross.CQRS.Queries;

/// <summary>
/// Base query handler.
/// </summary>
public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <inheritdoc />
    public Task<TResult> Handle(TQuery request, CancellationToken cancellationToken) => HandleAsync(request, cancellationToken);

    protected abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
