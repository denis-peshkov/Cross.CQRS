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
    public Task Handle(TCommandEvent commandEvent, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace(commandEvent, "Handling of the CommandEventType: {CommandEventType} for CommandId: {CommandId} has begun.", commandEvent.GetGenericTypeName(), commandEvent.CommandId);
        var sw = new Stopwatch();
        sw.Start();
        var result = HandleAsync(commandEvent, cancellationToken);
        sw.Stop();
        Logger.InternalLogTrace(commandEvent, "Handling of the CommandEventType: {CommandEventType} for CommandId: {CommandId} has completed successfully for a {ElapsedMilliseconds} ms.", commandEvent.GetGenericTypeName(), commandEvent.CommandId, sw.ElapsedMilliseconds);
        return result;
    }

    /// <summary>
    /// Defines a handler logic for a notification
    /// </summary>
    /// <param name="commandEvent">Command Event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response</returns>
    protected abstract Task HandleAsync(TCommandEvent commandEvent, CancellationToken cancellationToken);
}
