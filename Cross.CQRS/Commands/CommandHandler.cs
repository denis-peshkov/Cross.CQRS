namespace Cross.CQRS.Commands;

/// <summary>
/// Base command handler.
/// </summary>
public abstract class CommandHandler<TCommand> : AsyncRequestHandlerBase<TCommand>
    where TCommand : ICommand
{
    protected ICommandEventQueueWriter CommandEvents { get; }

    protected CommandHandler(ICommandEventQueueWriter commandEvents)
    {
        CommandEvents = commandEvents;
    }

    /// <inheritdoc />
    public override Task Handle(TCommand command, CancellationToken cancellationToken) => HandleAsync(command, cancellationToken);

    protected abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
