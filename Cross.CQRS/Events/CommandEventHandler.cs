namespace Cross.CQRS.Events;

/// <summary>
/// Wrapper class for a handler that handles an events and does not return a response
/// </summary>
public abstract class CommandEventHandler<TCommandEvent> : INotificationHandler<TCommandEvent>
	where TCommandEvent : INotification
{
	/// <summary>
	/// Override in a derived class for the handler logic
	/// </summary>
	/// <param name="commandEvent">Command Event</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Response</returns>
	public Task Handle(TCommandEvent commandEvent, CancellationToken cancellationToken) => HandleAsync(commandEvent, cancellationToken);

	protected abstract Task HandleAsync(TCommandEvent commandEvent, CancellationToken cancellationToken);
}
