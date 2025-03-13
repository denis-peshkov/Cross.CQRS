namespace Cross.CQRS.Events;

public interface ICommandEvent : INotification, IInternalLogObject
{
    /// <summary>
    /// Identifier of source which led to creation of event, e.g. <see cref="ICommand.CommandId"/>.
    /// </summary>
    Guid CommandId { get; }

    /// <summary>
    /// Define a type of Event, should process event inside or outside of transaction.
    /// </summary>
    /// <returns></returns>
    CommandEventFlowTypeEnum EventFlowType()
        => CommandEventFlowTypeEnum.StandardFlow;
}
