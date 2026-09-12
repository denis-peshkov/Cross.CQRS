namespace Cross.CQRS.Tests.Registration;

public class RegistrationAndBehaviorTests
{
    [Test]
    public void AddCQRS_Throws_WhenNoAssembliesRegistered()
    {
        var services = new ServiceCollection();

        var act = () => services.AddCQRS(_ => { });
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void AddCQRS_RegistersCoreServicesAndPipelineOrder()
    {
        var services = new ServiceCollection();

        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<TestRequest>());

        services.Should().Contain(d => d.ServiceType == typeof(IHandlerLocator));
        services.Should().Contain(d => d.ServiceType == typeof(ICommandEventQueue));
        services.Should().Contain(d => d.ImplementationType != null && d.ImplementationType.Name == "LicenseCheckBehavior`2");

        var pipeline = services
            .Where(d => d.ServiceType == typeof(IPipelineBehavior<,>))
            .Select(d => d.ImplementationType)
            .Where(t => t != null)
            .ToArray();

        var pipelineNames = pipeline.Select(t => t!.Name).ToArray();
        pipelineNames.Should().Contain("LicenseCheckBehavior`2");
        pipelineNames.Should().Contain("CommandEventQueueProcessBehavior`2");
        pipelineNames.Should().Contain("RequestFilterBehavior`2");
        pipelineNames.Should().Contain("ValidationBehavior`2");
        pipelineNames.Should().Contain("ResultFilterBehavior`2");
        pipelineNames.Should().ContainInConsecutiveOrder(
            "LicenseCheckBehavior`2",
            "CommandEventQueueProcessBehavior`2",
            "RequestFilterBehavior`2",
            "ValidationBehavior`2",
            "ResultFilterBehavior`2");
    }

    [Test]
    public void BehaviorCollection_ReordersBehaviorsAndKeepsSingleDescriptors()
    {
        var services = new ServiceCollection();
        var behaviorCollection = new BehaviorCollection(services);

        behaviorCollection
            .AddBehavior(Type.GetType("Cross.CQRS.Behaviors.ValidationBehavior`2, Cross.CQRS")!, 2)
            .AddBehavior(Type.GetType("Cross.CQRS.Behaviors.RequestFilterBehavior`2, Cross.CQRS")!, 1)
            .AddBehavior(Type.GetType("Cross.CQRS.Behaviors.ResultFilterBehavior`2, Cross.CQRS")!, 3);

        var pipeline = services
            .Where(d => d.ServiceType == typeof(IPipelineBehavior<,>))
            .Select(d => d.ImplementationType)
            .Where(t => t != null)
            .ToArray();

        pipeline.Select(t => t!.Name).Should().Equal(
            new[]
            {
                "RequestFilterBehavior`2",
                "ValidationBehavior`2",
                "ResultFilterBehavior`2"
            });
    }

    [Test]
    public void HandlerLocator_FindsHandlerType_ForRegisteredRequest()
    {
        var services = new ServiceCollection();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<TestRequest>());
        var provider = services.BuildServiceProvider();

        var locator = provider.GetRequiredService<IHandlerLocator>();
        var handlerType = locator.FindHandlerTypeByRequest(typeof(TestRequest));

        handlerType.Should().Be(typeof(TestRequestHandler));
    }

    [Test]
    public void HandlerLocator_Throws_ForNonRequestType()
    {
        var services = new ServiceCollection();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<TestRequest>());
        var provider = services.BuildServiceProvider();
        var locator = provider.GetRequiredService<IHandlerLocator>();

        var act = () => locator.FindHandlerTypeByRequest(typeof(string));
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void HandlerLocator_Throws_WhenRequestHasNoRegisteredHandler()
    {
        var services = new ServiceCollection();
        services.AddCQRS(cfg => cfg.RegisterFromAssemblyContaining<TestRequest>());
        var provider = services.BuildServiceProvider();
        var locator = provider.GetRequiredService<IHandlerLocator>();

        var act = () => locator.FindHandlerTypeByRequest(typeof(UnhandledRequest));
        act.Should().Throw<InvalidOperationException>();
    }

    public sealed record TestRequest(string Value) : IRequest<string>;

    public sealed class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken) => Task.FromResult(request.Value);
    }

    public sealed record UnhandledRequest(string Value) : IRequest<string>;
}
