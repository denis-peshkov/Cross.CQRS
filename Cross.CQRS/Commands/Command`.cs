namespace Cross.CQRS.Commands;

/// <summary>
/// Base implementation of <see cref="ICommand{TResult}"/>.
/// </summary>
public abstract class Command<TResult> : ICommand<TResult>
{
    /// <inheritdoc />
    public Guid CommandId { get; } = Guid.NewGuid();
}
