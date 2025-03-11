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
    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has begun.", command.GetGenericTypeName(), command.CommandId);
        var sw = new Stopwatch();
        sw.Start();
        var result = await HandleAsync(command, cancellationToken);
        sw.Stop();
        Logger.InternalLogTrace(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has completed successfully for a {ElapsedMilliseconds} ms.", command.GetGenericTypeName(), command.CommandId, sw.ElapsedMilliseconds);
        return result;
    }

    protected abstract Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
