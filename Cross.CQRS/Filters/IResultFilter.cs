namespace Cross.CQRS.Filters;

/// <summary>
/// Defines a filter for a particular type.
/// </summary>
/// <remarks>
///     Use it if you need to perform filter operation on the <see cref="TResult"/>,
///     after the query <see cref="IQuery{TResponse}"/> or command <see cref="ICommand"/>.
/// </remarks>
public interface IResultFilter<TRequest, TResult>
{
	/// <summary>
	/// Transform the handler result synchronously.
	/// </summary>
	/// <param name="result">Value returned by the handler (the instance to filter).</param>
	/// <returns>Transformed result value.</returns>
	TResult ApplyFilter(TResult result);

	/// <summary>
	/// Transform the handler result asynchronously.
	/// </summary>
	/// <param name="result">Value returned by the handler (the instance to filter).</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Transformed result value.</returns>
	Task<TResult> ApplyFilterAsync(TResult result, CancellationToken cancellationToken);
}
