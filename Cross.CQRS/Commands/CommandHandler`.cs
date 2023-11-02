namespace Cross.CQRS.Commands;

/// <summary>
/// Base command handler.
/// </summary>
public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected IEventQueueWriter Events { get; }

    protected CommandHandler(IEventQueueWriter events)
    {
        Events = events;
    }

    /// <inheritdoc />
    public Task<TResult> Handle(TCommand request, CancellationToken cancellationToken) => HandleAsync(request, cancellationToken);

    protected abstract Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
