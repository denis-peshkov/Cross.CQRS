namespace Cross.CQRS.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Cross.CQRS services with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configure">Configuration action for CQRS options (e.g. LicenseKey, RegisterFromAssembly).</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static CqrsRegistrationSyntax AddCQRS(this IServiceCollection services, Action<CqrsServiceConfiguration> configure)
    {
        var options = new CqrsServiceConfiguration();
        configure(options);

        if (options.Assemblies.Count == 0)
        {
            throw new InvalidOperationException("At least one assembly must be registered. Use cfg.RegisterFromAssembly(typeof(Startup).Assembly) or cfg.RegisterFromAssemblyContaining<Startup>().");
        }

        return AddCQRSInternal(services, options);
    }

    private static CqrsRegistrationSyntax AddCQRSInternal(IServiceCollection services, CqrsServiceConfiguration serviceConfiguration)
    {
        var assemblies = serviceConfiguration.Assemblies.ToArray();
        var behaviorCollection = new BehaviorCollection(services);

        LicenseChecked = false;
        services.AddSingleton(serviceConfiguration);

        services.AddSingleton<LicenseAccessor>();
        services.AddSingleton<LicenseValidator>();

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
        // services.AddMediatR( // v. 12.5.0
        //     cfg =>
        //     {
        //         cfg.Lifetime = ServiceLifetime.Scoped;
        //         cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher); // ForeachAwaitPublisher, TaskWhenAllPublisher
        //         cfg.RegisterServicesFromAssemblies(assemblies);
        //     });

        services.AddSingleton<IHandlerLocator>(_ => new HandlerLocator(services));

        services.AddScoped<ICommandEventQueue, CommandEventQueue>();
        services.AddScoped(sp => sp.GetRequiredService<ICommandEventQueue>().Reader);
        services.AddScoped(sp => sp.GetRequiredService<ICommandEventQueue>().Writer);

        // Changed to use IRequestPreProcessor instead of IPipelineBehavior for validation
        // services.AddScoped(typeof(IRequestPreProcessor<>), typeof(ValidationBehavior<>));

        // Registration order is important, it works like ASP.NET Core middleware
        // Behaviors registered earlier will be executed earlier
        behaviorCollection.AddBehavior(typeof(LicenseCheckBehavior<,>), order: -1); // License check runs first and is mandatory for every CQRS request
        behaviorCollection.AddBehavior(typeof(CommandEventQueueProcessBehavior<,>), order: 0);
        behaviorCollection.AddBehavior(typeof(RequestFilterBehavior<,>), order: 1);
        behaviorCollection.AddBehavior(typeof(ValidationBehavior<,>), order: 2);
        behaviorCollection.AddBehavior(typeof(ResultFilterBehavior<,>), order: 3);

        return new CqrsRegistrationSyntax(services, assemblies, behaviorCollection);
    }

    internal static void CheckLicense(this IServiceProvider serviceProvider)
    {
        if (LicenseChecked == false)
        {
            var licenseAccessor = serviceProvider.GetRequiredService<LicenseAccessor>();
            var licenseValidator = serviceProvider.GetRequiredService<LicenseValidator>();
            var license = licenseAccessor.Current;
            licenseValidator.Validate(license);
        }

        // if True then check will be performed only once
        LicenseChecked = false;
    }

    internal static bool LicenseChecked { get; set; }
}
