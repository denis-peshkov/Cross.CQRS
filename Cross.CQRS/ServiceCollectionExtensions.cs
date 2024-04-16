namespace Cross.CQRS;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers required services from the specified assemblies to the
    /// specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="assemblies">Assemblies to scan</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static CqrsRegistrationSyntax AddCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        var behaviorCollection = new BehaviorCollection(services);

        // FluentValidations
        services.AddValidatorsFromAssembly(assemblies.FirstOrDefault(), ServiceLifetime.Scoped, result =>
        {
            var isNoRegisterAutomatically = result.ValidatorType
                .GetCustomAttributes(typeof(NoRegisterAutomaticallyAttribute), inherit: false)
                .Length != 0;

            return !isNoRegisterAutomatically;
        });

        // Filters
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IResultFilter<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestFilter<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddMediatR(o => o.AsScoped(), assemblies);

        services.AddSingleton<IHandlerLocator>(_ => new HandlerLocator(services));

        services.AddScoped<IEventQueue, EventQueue>();
        services.AddScoped(sp => sp.GetRequiredService<IEventQueue>().Reader);
        services.AddScoped(sp => sp.GetRequiredService<IEventQueue>().Writer);

        // Registration order is important, it works like ASP.NET Core middleware
        // Behaviors registered earlier will be executed earlier
        behaviorCollection.AddBehavior(typeof(EventQueueProcessBehavior<,>), order: int.MinValue);
        behaviorCollection.AddBehavior(typeof(ValidationBehavior<,>), order: 1);
        behaviorCollection.AddBehavior(typeof(RequestFilterBehavior<,>), order: 2);
        behaviorCollection.AddBehavior(typeof(ResultFilterBehavior<,>), order: 3);

        return new CqrsRegistrationSyntax(services, assemblies, behaviorCollection);
    }
}
