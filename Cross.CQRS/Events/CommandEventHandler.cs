namespace Cross.CQRS.Events;

/// <summary>
/// Wrapper class for a handler that handles an events and does not return a response
/// </summary>
public abstract class CommandEventHandler<TCommandEvent> : INotificationHandler<TCommandEvent>
    where TCommandEvent : ICommandEvent
{
    protected ILogger<CommandEventHandler<TCommandEvent>> Logger { get; }

    protected CommandEventHandler(ILogger<CommandEventHandler<TCommandEvent>> logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Override in a derived class for the handler logic
    /// </summary>
    /// <param name="commandEvent">Command Event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response</returns>
    public async Task Handle(TCommandEvent commandEvent, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace<TCommandEvent>(commandEvent, "Handling of the CommandEventType: {CommandEventType} for CommandId: {CommandId} has begun.", commandEvent.GetGenericTypeName(), commandEvent.CommandId);
        var start = Stopwatch.GetTimestamp();
        await HandleAsync(commandEvent, cancellationToken);
        var elapsed = StopwatchHelper.GetElapsedMilliseconds(start);
        Logger.InternalLogTrace<TCommandEvent>(commandEvent, "Handling of the CommandEventType: {CommandEventType} for CommandId: {CommandId} has completed successfully in {Elapsed} ms.", commandEvent.GetGenericTypeName(), commandEvent.CommandId, elapsed);
    }

    /// <summary>
    /// Defines a handler logic for a notification
    /// </summary>
    /// <param name="commandEvent">Command Event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response</returns>
    protected abstract Task HandleAsync(TCommandEvent commandEvent, CancellationToken cancellationToken);
}
