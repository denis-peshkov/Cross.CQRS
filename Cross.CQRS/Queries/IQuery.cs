namespace Cross.CQRS.Queries;

/// <summary>
/// Marker interfaces represent a read-request.
/// </summary>
/// <remarks>
///     Use it if you need to perform read operation without change system state.
///     Do not use it to change database entities, instead use <see cref="ICommand"/>.
/// </remarks>
/// <typeparam name="TResult">Query result.</typeparam>
public interface IQuery<out TResult> : IRequest<TResult>, IInternalLogObject
{
    /// <summary>
    /// Unique query identifier.
    /// </summary>
    Guid QueryId { get; }
}
