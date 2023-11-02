namespace Cross.CQRS.Services;

internal sealed class HandlerLocator : IHandlerLocator
{
    private static readonly ConcurrentDictionary<Type, Type> _typeCache = new();

    private readonly IServiceCollection _serviceCollection;

    public HandlerLocator(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    /// <inheritdoc />
    public Type FindHandlerTypeByRequest(Type requestType)
    {
        if (requestType == null)
        {
            throw new ArgumentNullException(nameof(requestType));
        }

        if (!typeof(IBaseRequest).IsAssignableFrom(requestType))
        {
            throw new ArgumentException($"Type '{requestType.FullName}' should implement IRequest", nameof(requestType));
        }

        return _typeCache.GetOrAdd(requestType, t =>
        {
            Type GetHandlerType()
            {
                var requestInterface = requestType.GetTypeInfo().ImplementedInterfaces
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));

                var responseType = requestInterface.GetGenericArguments()[0];
                return typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
            }

            var handlerType = GetHandlerType();
            var descriptor = _serviceCollection.FirstOrDefault(s => s.ServiceType == handlerType);

            return descriptor?.ImplementationType;
        });
    }
}
