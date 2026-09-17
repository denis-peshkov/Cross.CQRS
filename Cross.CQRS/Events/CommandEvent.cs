namespace Cross.CQRS.Events;

/// <summary>
/// Base implementation of <see cref="ICommandEvent"/>.
/// </summary>
public abstract record CommandEvent : ICommandEvent
{
    protected CommandEvent(Guid commandId)
    {
        CommandId = commandId;
    }

    /// <inheritdoc />
    public Guid CommandId { get; }

    /// <inheritdoc />
    public virtual CommandEventFlowTypeEnum EventFlowType() => CommandEventFlowTypeEnum.StandardFlow;
}
