namespace Cross.CQRS.Events;

public interface IEventQueue
{
    IEventQueueWriter Writer { get; }

    IEventQueueReader Reader { get; }
}
