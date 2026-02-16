namespace Cross.CQRS.Licensing;

internal class LicenseValidator
{
    private readonly ILogger _logger;

    public LicenseValidator(ILoggerFactory loggerFactory)
        => _logger = loggerFactory.CreateLogger("Peshkov.Cross.CQRS.License");

    public void Validate(License license)
    {
        var errors = new List<string>();

        if (license is not { IsConfigured: true })
        {
            var message = "You do not have a valid license key for the Peshkov software Cross.CQRS. " +
                          "This is allowed for development and testing scenarios. " +
                          "If you are running in production you are required to have a licensed version. " +
                          "Please visit https://peshkov.biz to obtain a valid license.";

            _logger.LogCritical(message);
            return;
        }

        _logger.LogDebug("The Peshkov software license key details: {License}", license);

        var diff = DateTime.UtcNow.Date.Subtract(license.ExpirationDate!.Value.Date).TotalDays;
        if (diff > 0)
        {
            errors.Add($"Your license for the Peshkov software Cross.CQRS expired {diff} days ago.");
        }

        if (license.ProductType!.Value != ProductTypeEnum.Cross_CQRS
            && license.ProductType.Value != ProductTypeEnum.Bundle)
        {
            errors.Add("Your Peshkov software license does not include Cross.CQRS.");
        }

        if (errors.Count > 0)
        {
            foreach (var err in errors)
            {
                _logger.LogError(err);
            }

            _logger.LogCritical("Please visit https://peshkov.biz to obtain a valid license for the Peshkov software Cross.CQRS.");
        }
        else
        {
            _logger.LogInformation("You have a valid license key for the Peshkov software {Type} {Edition} edition. The license expires on {LicenseExpiration}.",
                license.ProductType,
                license.Edition,
                license.ExpirationDate);
        }
    }
}
