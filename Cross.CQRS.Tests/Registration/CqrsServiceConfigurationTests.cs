namespace Cross.CQRS.Tests.Registration;

public class CqrsServiceConfigurationTests
{
    [Test]
    public void RegisterFromAssemblies_AddsDistinctAssemblies()
    {
        var cfg = new CqrsServiceConfiguration();
        var assembly = typeof(CqrsServiceConfigurationTests).Assembly;

        cfg.RegisterFromAssemblies(assembly, assembly);

        cfg.Assemblies.Should().ContainSingle();
    }

    [Test]
    public void RegisterFromAssemblyContaining_AddsAssembly()
    {
        var cfg = new CqrsServiceConfiguration();

        cfg.RegisterFromAssemblyContaining<CqrsServiceConfigurationTests>();

        cfg.Assemblies.Should().Contain(typeof(CqrsServiceConfigurationTests).Assembly);
    }
}
