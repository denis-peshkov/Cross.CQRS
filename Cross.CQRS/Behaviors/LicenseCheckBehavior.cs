namespace Cross.CQRS.Behaviors;

internal sealed class LicenseCheckBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IServiceProvider _serviceProvider;

    public LicenseCheckBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        _serviceProvider.CheckLicense();
        return next();
    }
}
