namespace Cross.CQRS.Filters;

/// <summary>
/// Defines a filter for a particular type.
/// </summary>
/// <remarks>
///     Use it if you need to perform filter operation on the <see cref="TRequest"/>,
///     after the query <see cref="IQuery{TResponse}"/> or command <see cref="ICommand"/>.
/// </remarks>
public interface IRequestFilter<TRequest>
{
    /// <summary>
    /// Filter the specified instance.
    /// </summary>
    /// <param name="result">The instance to filter</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    TRequest ApplyFilter(TRequest result);

    /// <summary>
    /// Filter the specified instance asynchronously.
    /// </summary>
    /// <param name="result">The instance to filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    Task<TRequest> ApplyFilterAsync(TRequest result, CancellationToken cancellationToken);
}