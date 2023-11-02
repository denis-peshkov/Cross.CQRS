namespace Cross.CQRS.Events;

public interface IEventQueueReader
{
    IReadOnlyCollection<IEvent> Read(Guid requestId);
}
