namespace Cross.CQRS.Services;

public interface IHandlerLocator
{
    Type FindHandlerTypeByRequest(Type requestType);
}
