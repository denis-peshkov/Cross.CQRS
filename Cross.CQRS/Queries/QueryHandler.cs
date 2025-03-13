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

        var isNoStopWatchMeasurement = request
            .GetType()
            .GetCustomAttributes(typeof(NoStopWatchMeasurementAttribute), inherit: true)
            .Length != 0;

        if (isNoStopWatchMeasurement)
        {
            var result = await HandleAsync(request, cancellationToken);
            Logger.InternalLogTrace<TResult>(request, "Handling of the QueryType: {QueryType} for QueryId: {QueryId} has completed successfully.", request.GetGenericTypeName(), request.QueryId);
            return result;
        }
        else
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = await HandleAsync(request, cancellationToken);
            sw.Stop();
            Logger.InternalLogTrace<TResult>(request, "Handling of the QueryType: {QueryType} for QueryId: {QueryId} has completed successfully for a {ElapsedMilliseconds} ms.", request.GetGenericTypeName(), request.QueryId, sw.ElapsedMilliseconds);
            return result;
        }
    }

    protected abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
