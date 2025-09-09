namespace Cross.CQRS.Commands;

/// <summary>
/// Base command handler.
/// </summary>
public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
    protected ICommandEventQueueWriter CommandEvents { get; }
    protected ILogger<CommandHandler<TCommand>> Logger { get; }

    protected CommandHandler(ICommandEventQueueWriter commandEvents, ILogger<CommandHandler<TCommand>> logger)
    {
        CommandEvents = commandEvents;
        Logger = logger;
    }

    /// <inheritdoc />
    async Task<Unit> IRequestHandler<TCommand, Unit>.Handle(TCommand command, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace<Unit>(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has begun.", command.GetGenericTypeName(), command.CommandId);
        var start = Stopwatch.GetTimestamp();
        await HandleAsync(command, cancellationToken);
        var elapsed = StopwatchHelper.GetElapsedMilliseconds(start);
        Logger.InternalLogTrace<Unit>(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has completed successfully for a {Elapsed} ms.", command.GetGenericTypeName(), command.CommandId, elapsed);
        return Unit.Value;
    }

    protected abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
