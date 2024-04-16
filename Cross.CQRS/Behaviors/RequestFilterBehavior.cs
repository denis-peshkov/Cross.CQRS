using Cross.CQRS.Filters;

namespace Cross.CQRS.Behaviors;

internal sealed class RequestFilterBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IEnumerable<IRequestFilter<TRequest>> _filters;

    public RequestFilterBehavior(IEnumerable<IRequestFilter<TRequest>> filters)
    {
        _filters = filters;
    }

    /// <inheritdoc />
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var filters = _filters.ToArray();
        if (filters.Length <= 0)
        {
            return await next();
        }

        var result = request;
        foreach (var filter in filters)
        {
            result = await filter.ApplyFilterAsync(result, cancellationToken);
        }

        return await next();
    }
}
