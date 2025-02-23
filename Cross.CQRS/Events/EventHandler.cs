namespace Cross.CQRS.Events;

/// <summary>
/// Wrapper class for a handler that handles an events and does not return a response
/// </summary>
public abstract class EventHandler<TEvent> : INotificationHandler<TEvent>
	where TEvent : INotification
{
	/// <summary>
	/// Override in a derived class for the handler logic
	/// </summary>
	/// <param name="event">Event</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Response</returns>
	public Task Handle(TEvent @event, CancellationToken cancellationToken) => HandleAsync(@event, cancellationToken);

	protected abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
