namespace Cross.CQRS.Events;

public interface ICommandEventQueueWriter
{
    void Write(ICommandEvent commandEvent);
}
