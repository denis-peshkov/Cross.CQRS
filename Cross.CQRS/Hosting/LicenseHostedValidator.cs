namespace Cross.CQRS.Hosting;

/// <summary>
/// Runs <c>CheckLicense</c> when the generic host starts, before the first MediatR request.
/// </summary>
internal sealed class LicenseHostedValidator : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public LicenseHostedValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _serviceProvider.CheckLicense();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
