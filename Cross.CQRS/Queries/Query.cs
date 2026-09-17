namespace Cross.CQRS.Queries;

/// <summary>
/// Base implementation of <see cref="IQuery{TResult}"/>.
/// </summary>
public abstract record Query<TResult> : IQuery<TResult>
{
    /// <inheritdoc />
    public Guid QueryId { get; } = Guid.NewGuid();
}
