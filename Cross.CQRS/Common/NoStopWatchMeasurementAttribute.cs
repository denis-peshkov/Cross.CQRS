namespace Cross.CQRS.Common;

/// <summary>
/// Marker attribute to avoid StopWatch measurement on handle the command / commandEvent / query.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class NoStopWatchMeasurementAttribute : Attribute
{
}
