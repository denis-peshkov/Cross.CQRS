using Cross.CQRS.Filters;

namespace Cross.CQRS.Behaviors;

internal sealed class ResultFilterBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IEnumerable<IResultFilter<TRequest, TResult>> _filters;

    public ResultFilterBehavior(IEnumerable<IResultFilter<TRequest, TResult>> filters)
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

        var responce = await next();

        var result = responce;
        foreach (var filter in filters)
        {
            result = await filter.ApplyFilterAsync(result, cancellationToken);
        }

        return result;
    }
}
