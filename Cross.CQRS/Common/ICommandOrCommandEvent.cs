namespace Cross.CQRS.Common;

public interface ICommandOrCommandEvent
{
    /// <summary>
    /// Unique command identifier.
    /// For CommandEvent - identifier of source which led to creation of event, e.g. <see cref="ICommand.CommandId"/>.
    /// </summary>
    Guid CommandId { get; }
}
