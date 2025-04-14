namespace Cross.CQRS.Extensions;

public static class StopwatchHelper
{
    public static long GetElapsedMilliseconds(long start)
    {
        var elapsed = (Stopwatch.GetTimestamp() - start) * 1000 / (double)Stopwatch.Frequency;

        return (long)elapsed;
    }
}
