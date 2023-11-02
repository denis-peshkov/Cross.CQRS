namespace Cross.CQRS;

public sealed class BehaviorCollection
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, int> _orderedPipelineBehaviors = new();

    public BehaviorCollection(IServiceCollection services)
    {
        _services = services;
    }

    public BehaviorCollection AddBehavior(Type behaviorType, int order)
    {
        // 1 remove all registered pipeline behaviors
        // 2 reorder according to registrations
        // 3 add all behaviors to service collection
        var existingDescriptors = _services
            .Where(d => d.ServiceType == typeof(IPipelineBehavior<,>) && _orderedPipelineBehaviors.ContainsKey(d.ImplementationType))
            .ToArray();

        foreach (var existingDescriptor in existingDescriptors)
        {
            _services.Remove(existingDescriptor);
        }

        _orderedPipelineBehaviors.Add(behaviorType, order);

        var orderedBehaviors = _orderedPipelineBehaviors
            .Select(b => new { Type = b.Key, Order = b.Value })
            .OrderBy(s => s.Order)
            .Select(s => s.Type)
            .ToArray();

        foreach (var behavior in orderedBehaviors)
        {
            _services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), behavior, ServiceLifetime.Scoped));
        }

        return this;
    }
}
