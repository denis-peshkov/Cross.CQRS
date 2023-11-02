namespace Cross.CQRS;

public static class ServiceCollectionExtensions
{
    public static CqrsRegistrationSyntax AddCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        var behaviorCollection = new BehaviorCollection(services);

        // FluentValidation
        services.AddValidatorsFromAssembly(assemblies.FirstOrDefault());

        services.AddMediatR(o => o.AsScoped(), assemblies);

        services.AddSingleton<IHandlerLocator>(_ => new HandlerLocator(services));

        services.AddScoped<IEventQueue, EventQueue>();
        services.AddScoped(sp => sp.GetRequiredService<IEventQueue>().Reader);
        services.AddScoped(sp => sp.GetRequiredService<IEventQueue>().Writer);

        // Registration order is important, it works like ASP.NET Core middleware
        // Behaviors registered earlier will be executed earlier
        behaviorCollection.AddBehavior(typeof(EventQueueProcessBehavior<,>), order: int.MinValue);
        behaviorCollection.AddBehavior(typeof(ValidationBehavior<,>), order: 1);

        return new CqrsRegistrationSyntax(services, assemblies, behaviorCollection);
    }
}
