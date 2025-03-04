namespace Cross.CQRS.Events;

public interface ICommandEventQueueReader
{
    IReadOnlyCollection<ICommandEvent> Read(Guid requestId);
}
