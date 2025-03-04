namespace Cross.CQRS.Events;

public interface ICommandEventQueue
{
    ICommandEventQueueWriter Writer { get; }

    ICommandEventQueueReader Reader { get; }
}
