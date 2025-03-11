namespace Cross.CQRS.Commands;

/// <summary>
/// Base command handler.
/// </summary>
public abstract class CommandHandler<TCommand> : AsyncRequestHandlerBase<TCommand>
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
    public override async Task Handle(TCommand command, CancellationToken cancellationToken)
    {
        Logger.InternalLogTrace(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has begun.", command.GetGenericTypeName(), command.CommandId);
        var sw = new Stopwatch();
        sw.Start();
        await HandleAsync(command, cancellationToken);
        sw.Stop();
        Logger.InternalLogTrace(command, "Handling of the CommandType: {CommandType} for CommandId: {CommandId} has completed successfully for a {ElapsedMilliseconds} ms.", command.GetGenericTypeName(), command.CommandId, sw.ElapsedMilliseconds);
    }

    protected abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
