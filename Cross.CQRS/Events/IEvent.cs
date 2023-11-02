namespace Cross.CQRS.Events;

public interface IEvent : INotification
{
    /// <summary>
    /// Identifier of source which led to creation of event, e.g. <see cref="ICommand.CommandId"/>.
    /// </summary>
    Guid CommandId { get; }
}
