namespace Cross.CQRS.Commands;

/// <summary>
/// Base command handler.
/// </summary>
public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected ICommandEventQueueWriter CommandEvents { get; }
    protected ILogger<CommandHandler<TCommand, TResult>> Logger { get; }

    protected CommandHandler(ICommandEventQueueWriter commandEvents, ILogger<CommandHandler<TCommand, TResult>> logger)
    {
        CommandEvents = commandEvents;
        Logger = logger;
    }

    /// <inheritdoc />
    async Task<TResult> IRequestHandler<TCommand, TResult>.Handle(TCommand command, CancellationToken cancellationToken)
    {
        return await Handle(command, cancellationToken);
    }

    /// <summary>
    /// Handles a command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response from the command</returns>
    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace<TResult>(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has begun.", command.GetGenericTypeName(), command.CommandId);
        var start = Stopwatch.GetTimestamp();
        var result = await HandleAsync(command, cancellationToken);
        var elapsed = StopwatchHelper.GetElapsedMilliseconds(start);
        Logger.InternalLogTrace<TResult>(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has completed successfully in {Elapsed} ms.", command.GetGenericTypeName(), command.CommandId, elapsed);
        return result;
    }

    protected abstract Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
