namespace Cross.CQRS.Commands;

/// <summary>
/// Base command handler.
/// </summary>
public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected ICommandEventQueueWriter CommandEvents { get; }

    protected CommandHandler(ICommandEventQueueWriter commandEvents)
    {
        CommandEvents = commandEvents;
    }

    /// <inheritdoc />
    public Task<TResult> Handle(TCommand request, CancellationToken cancellationToken) => HandleAsync(request, cancellationToken);

    protected abstract Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
