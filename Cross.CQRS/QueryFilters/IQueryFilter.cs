namespace Cross.CQRS.QueryFilters;

/// <summary>
/// Defines a filter for a particular type.
/// </summary>
/// <remarks>
///     Use it if you need to perform filter operation on the <see cref="TResult"/>,
///     after the query <see cref="IQuery{TResponse}"/>.
/// </remarks>
public interface IQueryFilter<TQuery, TResult>
{
	/// <summary>
	/// Filter the specified instance.
	/// </summary>
	/// <param name="response">The instance to filter</param>
	/// <returns>A ValidationResult object containing any validation failures.</returns>
	TResult Filter(TResult response);

	/// <summary>
	/// Filter the specified instance asynchronously.
	/// </summary>
	/// <param name="response">The instance to filter</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>A ValidationResult object containing any validation failures.</returns>
	Task<TResult> FilterAsync(TResult response, CancellationToken cancellationToken);
}
