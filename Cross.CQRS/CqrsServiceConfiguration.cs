namespace Cross.CQRS;

/// <summary>
/// Configuration options for Cross.CQRS registration.
/// </summary>
public class CqrsServiceConfiguration
{
    /// <summary>
    /// License key for Peshkov software Cross.CQRS.
    /// </summary>
    public string? LicenseKey { get; set; }

    /// <summary>
    /// Assemblies to scan for handlers, validators and filters.
    /// </summary>
    public ICollection<Assembly> Assemblies { get; } = new List<Assembly>();

    /// <summary>
    /// Registers the specified assembly for scanning.
    /// </summary>
    public CqrsServiceConfiguration RegisterFromAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            if (!Assemblies.Contains(assembly))
            {
                Assemblies.Add(assembly);
            }
        }

        return this;
    }

    /// <summary>
    /// Registers the assembly containing the specified type for scanning.
    /// </summary>
    public CqrsServiceConfiguration RegisterFromAssemblyContaining<T>()
    {
        RegisterFromAssemblies(typeof(T).Assembly);
        return this;
    }
}
