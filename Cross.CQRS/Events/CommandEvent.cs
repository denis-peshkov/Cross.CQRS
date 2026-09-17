namespace Cross.CQRS.Events;

/// <summary>
/// Base implementation of <see cref="ICommandEvent"/>.
/// </summary>
public abstract record CommandEvent : ICommandEvent
{
    /// <inheritdoc />
    public Guid CommandEventId { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public Guid CommandId { get; }

    /// <inheritdoc />
    public virtual CommandEventFlowTypeEnum EventFlowType() => CommandEventFlowTypeEnum.StandardFlow;

    protected CommandEvent(Guid commandId)
    {
        CommandId = commandId;
    }
}
