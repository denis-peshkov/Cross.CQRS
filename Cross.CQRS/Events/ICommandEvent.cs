namespace Cross.CQRS.Events;

public interface ICommandEvent : INotification, ICommandOrCommandEvent
{
    /// <summary>
    /// Define a type of Event, should process event inside or outside of transaction.
    /// </summary>
    /// <returns></returns>
    CommandEventFlowTypeEnum EventFlowType()
        => CommandEventFlowTypeEnum.StandardFlow;
}
