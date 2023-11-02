namespace Cross.CQRS.Commands;

/// <summary>
/// Wrapper class for a handler that asynchronously handles a request and does not return a response
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
public abstract class AsyncRequestHandlerBase<TCommnd> : IRequestHandler<TCommnd>
	where TCommnd : IRequest
{
	async Task<Unit> IRequestHandler<TCommnd, Unit>.Handle(TCommnd command, CancellationToken cancellationToken)
	{
		await Handle(command, cancellationToken).ConfigureAwait(false);
		return Unit.Value;
	}

	/// <summary>
	/// Override in a derived class for the handler logic
	/// </summary>
	/// <param name="command">Request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Response</returns>
	public abstract Task Handle(TCommnd command, CancellationToken cancellationToken);
}
