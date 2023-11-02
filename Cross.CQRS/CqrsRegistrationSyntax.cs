namespace Cross.CQRS;

public class CqrsRegistrationSyntax
{
    public IServiceCollection Services { get; }

    public ICollection<Assembly> Assemblies { get; }

    public BehaviorCollection Behaviors { get; }

    public CqrsRegistrationSyntax(IServiceCollection services, ICollection<Assembly> assemblies, BehaviorCollection behaviors)
    {
        Services = services;
        Assemblies = assemblies;
        Behaviors = behaviors;
    }
}
