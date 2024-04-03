namespace Cross.CQRS.Behaviors;

/// <summary>
/// Marker attribute to avoid register instance of class automatically in DI.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class NoRegisterAutomaticallyAttribute : Attribute
{
}
