namespace Cross.CQRS.Tests;

public class LicensingTests
{
    [Test]
    public void License_IsConfigured_True_WhenAllClaimsPresent()
    {
        var now = DateTimeOffset.UtcNow;
        var claims = new[]
        {
            new Claim("sub_id", Guid.NewGuid().ToString()),
            new Claim("user_id", Guid.NewGuid().ToString()),
            new Claim("iat", now.ToUnixTimeSeconds().ToString()),
            new Claim("nbf", now.AddMinutes(-1).ToUnixTimeSeconds().ToString()),
            new Claim("exp", now.AddDays(1).ToUnixTimeSeconds().ToString()),
            new Claim("edition", EditionEnum.Professional.ToString()),
            new Claim("type", ProductTypeEnum.Cross_CQRS.ToString())
        };

        var license = new License(claims);

        license.IsConfigured.Should().BeTrue();
        license.Edition.Should().Be(EditionEnum.Professional);
        license.ProductType.Should().Be(ProductTypeEnum.Cross_CQRS);
    }

    [Test]
    public void License_IsConfigured_False_WhenClaimsMissing()
    {
        var license = new License(Array.Empty<Claim>());
        license.IsConfigured.Should().BeFalse();
    }

    [Test]
    public void LicenseValidator_LogsCritical_WhenLicenseNotConfigured()
    {
        var sink = new TestLoggerProvider();
        var factory = LoggerFactory.Create(b => b.AddProvider(sink));
        var validator = new LicenseValidator(factory);

        validator.Validate(new License(Array.Empty<Claim>()));

        sink.Entries.Should().Contain(e => e.Level == LogLevel.Critical && e.Message.Contains("valid license key"));
    }

    [Test]
    public void LicenseValidator_LogsInformation_WhenLicenseValid()
    {
        var now = DateTimeOffset.UtcNow;
        var claims = new[]
        {
            new Claim("sub_id", Guid.NewGuid().ToString()),
            new Claim("user_id", Guid.NewGuid().ToString()),
            new Claim("iat", now.ToUnixTimeSeconds().ToString()),
            new Claim("nbf", now.AddMinutes(-1).ToUnixTimeSeconds().ToString()),
            new Claim("exp", now.AddDays(10).ToUnixTimeSeconds().ToString()),
            new Claim("edition", EditionEnum.Enterprise.ToString()),
            new Claim("type", ProductTypeEnum.Cross_CQRS_EF.ToString())
        };

        var sink = new TestLoggerProvider();
        var factory = LoggerFactory.Create(b => b.AddProvider(sink));
        var validator = new LicenseValidator(factory);

        validator.Validate(new License(claims));

        sink.Entries.Should().Contain(e => e.Level == LogLevel.Information && e.Message.Contains("valid license key"));
    }

    [Test]
    public void LicenseValidator_ValidateForEfExtension_LogsCritical_WhenLicenseNotConfigured_AndDoesNotThrow()
    {
        var sink = new TestLoggerProvider();
        var factory = LoggerFactory.Create(b => b.AddProvider(sink));
        var validator = new LicenseValidator(factory);

        var act = () => validator.ValidateForEfExtension(new License(Array.Empty<Claim>()));

        act.Should().NotThrow();
        sink.Entries.Should().Contain(e => e.Level == LogLevel.Critical && e.Message.Contains("valid license key"));
    }

    [Test]
    public void LicenseValidator_ValidateForEfExtension_Throws_WhenProductIsCrossCqrsOnly()
    {
        var now = DateTimeOffset.UtcNow;
        var claims = new[]
        {
            new Claim("sub_id", Guid.NewGuid().ToString()),
            new Claim("user_id", Guid.NewGuid().ToString()),
            new Claim("iat", now.ToUnixTimeSeconds().ToString()),
            new Claim("nbf", now.AddMinutes(-1).ToUnixTimeSeconds().ToString()),
            new Claim("exp", now.AddDays(10).ToUnixTimeSeconds().ToString()),
            new Claim("edition", EditionEnum.Enterprise.ToString()),
            new Claim("type", ProductTypeEnum.Cross_CQRS.ToString())
        };

        var sink = new TestLoggerProvider();
        var factory = LoggerFactory.Create(b => b.AddProvider(sink));
        var validator = new LicenseValidator(factory);

        var act = () => validator.ValidateForEfExtension(new License(claims));

        act.Should().Throw<InvalidOperationException>().WithMessage("*Cross_CQRS_EF*");
    }

    [Test]
    public void LicenseValidator_ValidateForEfExtension_DoesNotThrow_WhenProductIsCrossCqrsEf()
    {
        var now = DateTimeOffset.UtcNow;
        var claims = new[]
        {
            new Claim("sub_id", Guid.NewGuid().ToString()),
            new Claim("user_id", Guid.NewGuid().ToString()),
            new Claim("iat", now.ToUnixTimeSeconds().ToString()),
            new Claim("nbf", now.AddMinutes(-1).ToUnixTimeSeconds().ToString()),
            new Claim("exp", now.AddDays(10).ToUnixTimeSeconds().ToString()),
            new Claim("edition", EditionEnum.Enterprise.ToString()),
            new Claim("type", ProductTypeEnum.Cross_CQRS_EF.ToString())
        };

        var sink = new TestLoggerProvider();
        var factory = LoggerFactory.Create(b => b.AddProvider(sink));
        var validator = new LicenseValidator(factory);

        var act = () => validator.ValidateForEfExtension(new License(claims));

        act.Should().NotThrow();
        sink.Entries.Should().Contain(e =>
            e.Level == LogLevel.Information && e.Message.Contains("Cross.CQRS.EF", StringComparison.Ordinal));
    }

    [Test]
    public void LicenseAccessor_ReturnsCachedInstance_AndLogsInvalidJwtFormat()
    {
        var sink = new TestLoggerProvider();
        var factory = LoggerFactory.Create(b => b.AddProvider(sink));
        var config = new CqrsServiceConfiguration { LicenseKey = "not-a-jwt" };
        var accessor = new LicenseAccessor(config, factory);

        var first = accessor.Current;
        var second = accessor.Current;

        first.Should().BeSameAs(second);
        first.IsConfigured.Should().BeFalse();
        sink.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("JWS or JWE"));
    }
}

internal sealed class TestLoggerProvider : ILoggerProvider
{
    public List<(LogLevel Level, string Message)> Entries { get; } = new();

    public ILogger CreateLogger(string categoryName) => new TestLogger(Entries);
    public void Dispose() { }

    private sealed class TestLogger : ILogger
    {
        private readonly List<(LogLevel Level, string Message)> _entries;

        public TestLogger(List<(LogLevel Level, string Message)> entries) => _entries = entries;
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
            => _entries.Add((logLevel, formatter(state, exception)));
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
