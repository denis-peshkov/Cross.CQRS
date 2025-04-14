namespace Cross.CQRS.Extensions;

public static class InternalLogExtensions
{
    private static int MaxSizeInBytes => 200 * 1024;

    public static void InternalLogTrace<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Trace, null, message, args);
    }

    public static void InternalLogTrace<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Trace, ex, message, args);
    }

    public static void InternalLogDebug<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Debug, null, message, args);
    }

    public static void InternalLogDebug<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Debug, ex, message, args);
    }

    public static void InternalLogInformation<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Information, null, message, args);
    }

    public static void InternalLogInformation<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Information, ex, message, args);
    }

    public static void InternalLogWarning<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Warning, null, message, args);
    }

    public static void InternalLogWarning<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Warning, ex, message, args);
    }

    public static void InternalLogError<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Error, null, message, args);
    }

    public static void InternalLogError<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Error, ex, message, args);
    }

    public static void InternalLogCritical<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Critical, null, message, args);
    }

    public static void InternalLogCritical<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, LogLevel.Critical, ex, message, args);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private static void InternalLog<TResult>(this ILogger logger, IInternalLogObject internalLogObject, LogLevel logLevel, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        logger.InternalLog<TResult>(internalLogObject, logLevel, null, message, args);
    }

    private static void InternalLog<TResult>(this ILogger logger, IInternalLogObject internalLogObject, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = internalLogObject.GetObjectSize();

        var nameOfInternalLogObject = string.Empty;
        KeyValuePair<string, object?> logPropertyId = new();
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                nameOfInternalLogObject = "CommandEvent";
                logPropertyId = new("CommandId", commandEvent.CommandId);
                break;
            case ICommand command:
                nameOfInternalLogObject = "Command";
                logPropertyId = new("CommandId", command.CommandId);
                break;
            case ICommand<TResult> commandGeneric:
                nameOfInternalLogObject = "Command";
                logPropertyId = new("CommandId", commandGeneric.CommandId);
                break;
            case IQuery<TResult> queryGeneric:
                nameOfInternalLogObject = "Query";
                logPropertyId = new("QueryId", queryGeneric.QueryId);
                break;
        }

        var logPropertiesExtended = new List<KeyValuePair<string, object?>>
        {
            new($"{nameOfInternalLogObject}Size", size),
            new($"{nameOfInternalLogObject}", size > MaxSizeInBytes ? null : JsonSerializer.Serialize<object>(internalLogObject)),
            new($"{nameOfInternalLogObject}Type", internalLogObject.GetGenericTypeName()),
            logPropertyId,
        };

        using (logger.BeginScope(logPropertiesExtended))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }
}
