namespace Cross.CQRS.Events;

public interface IEventQueueWriter
{
    void Write(IEvent @event);
}
