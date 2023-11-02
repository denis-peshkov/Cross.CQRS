namespace Cross.CQRS.Commands;

/// <summary>
/// Marker interface represent a write-request.
/// </summary>
/// <remarks>
///     Use it to change system state e.g. add something or update.
///     Do not use it query data from database, instead use <see cref="IQuery{TResult}"/>.
/// </remarks>
public interface ICommand : ICommand<Unit>, IRequest
{
}
