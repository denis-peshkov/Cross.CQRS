namespace Cross.CQRS.Queries;

/// <summary>
/// Base query handler.
/// </summary>
public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    protected ILogger<QueryHandler<TQuery, TResult>> Logger { get; }

    protected QueryHandler(ILogger<QueryHandler<TQuery, TResult>> logger)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace<TResult>(request, "Handling of the QueryType: {QueryType} for QueryId: {QueryId} has begun.", request.GetGenericTypeName(), request.QueryId);
        var start = Stopwatch.GetTimestamp();
        var result = await HandleAsync(request, cancellationToken);
        var elapsed = StopwatchHelper.GetElapsedMilliseconds(start);
        Logger.InternalLogTrace<TResult>(request, "Handling of the QueryType: {QueryType} for QueryId: {QueryId} has completed successfully in {Elapsed} ms.", request.GetGenericTypeName(), request.QueryId, elapsed);
        return result;
    }

    protected abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
