namespace Cross.CQRS.Queries;

/// <summary>
/// Base implementation of <see cref="IQuery{TResult}"/>.
/// </summary>
public abstract class Query<TResult> : IQuery<TResult>
{
    /// <inheritdoc />
    public Guid QueryId { get; } = Guid.NewGuid();
}
