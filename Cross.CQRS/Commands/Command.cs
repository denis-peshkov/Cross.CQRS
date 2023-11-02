namespace Cross.CQRS.Commands;

/// <summary>
/// Base implementation of <see cref="ICommand{TResult}"/>.
/// </summary>
public abstract class Command : Command<Unit>, ICommand
{
}
