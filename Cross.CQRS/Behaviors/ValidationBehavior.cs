namespace Cross.CQRS.Behaviors;

internal sealed class ValidationBehavior<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : IRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc />
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var validators = _validators.ToList();
        if (!validators.Any())
        {
            return;
        }

        var results = new List<ValidationResult>();
        var context = new ValidationContext<TRequest>(request);

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(context, cancellationToken);
            results.Add(result);
        }

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(r => r != null)
            .ToArray();

        if (failures.Length > 0)
        {
            throw new ValidationException(failures);
        }
    }
}
