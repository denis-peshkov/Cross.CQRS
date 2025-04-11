namespace Cross.CQRS.Extensions;

public static class InternalLogExtensions
{
    private static int MaxSizeInBytes => 200 * 1024;

    public static void InternalLogTrace<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Trace, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Trace, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Trace, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Trace, null, message, args);
                break;
        }
    }

    public static void InternalLogTrace<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Trace, ex, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Trace, ex, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Trace, ex, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Trace, ex, message, args);
                break;
        }
    }

    public static void InternalLogDebug<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Debug, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Debug, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Debug, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Debug, null, message, args);
                break;
        }
    }

    public static void InternalLogDebug<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Debug, ex, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Debug, ex, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Debug, ex, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Debug, ex, message, args);
                break;
        }
    }

    public static void InternalLogInformation<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Information, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Information, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Information, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Information, null, message, args);
                break;
        }
    }

    public static void InternalLogInformation<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Information, ex, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Information, ex, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Information, ex, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Information, ex, message, args);
                break;
        }
    }

    public static void InternalLogWarning<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Warning, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Warning, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Warning, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Warning, null, message, args);
                break;
        }
    }

    public static void InternalLogWarning<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Warning, ex, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Warning, ex, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Warning, ex, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Warning, ex, message, args);
                break;
        }
    }

    public static void InternalLogError<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Error, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Error, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Error, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Error, null, message, args);
                break;
        }
    }

    public static void InternalLogError<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Error, ex, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Error, ex, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Error, ex, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Error, ex, message, args);
                break;
        }
    }

    public static void InternalLogCritical<TResult>(this ILogger logger, IInternalLogObject internalLogObject, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Critical, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Critical, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Critical, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Critical, null, message, args);
                break;
        }
    }

    public static void InternalLogCritical<TResult>(this ILogger logger, IInternalLogObject internalLogObject, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, LogLevel.Critical, ex, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, LogLevel.Critical, ex, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric, LogLevel.Critical, ex, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, LogLevel.Critical, ex, message, args);
                break;
        }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private static void InternalLog<TResult>(this ILogger logger, IInternalLogObject internalLogObject, LogLevel logLevel, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (internalLogObject)
        {
            case ICommandEvent commandEvent:
                logger.InternalLog(commandEvent, logLevel, null, message, args);
                break;
            case ICommand command:
                logger.InternalLog(command, logLevel, null, message, args);
                break;
            case ICommand<TResult> commandGeneric:
                logger.InternalLog(commandGeneric,logLevel, null, message, args);
                break;
            case IQuery<TResult> queryGeneric:
                logger.InternalLog(queryGeneric, logLevel, null, message, args);
                break;
        }
    }

    private static void InternalLog(this ILogger logger, ICommandEvent ev, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = ev.GetObjectSize();

        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("CommandEventSize", size),
                   new("CommandEvent", size > MaxSizeInBytes ? null : JsonSerializer.Serialize(ev)),
                   new("CommandEventType", ev.GetGenericTypeName()),
                   new("CommandId", ev.CommandId),
               }))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }

    private static void InternalLog(this ILogger logger, ICommand command, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = command.GetObjectSize();

        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("CommandSize", size),
                   new("Command", size > MaxSizeInBytes ? null : JsonSerializer.Serialize(command)),
                   new("CommandType", command.GetGenericTypeName()),
                   new("CommandId", command.CommandId),
               }))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }

    private static void InternalLog<T>(this ILogger logger, ICommand<T> command, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = command.GetObjectSize();

        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("CommandSize", size),
                   new("Command", size > MaxSizeInBytes ? null : JsonSerializer.Serialize(command)),
                   new("CommandType", command.GetGenericTypeName()),
                   new("CommandId", command.CommandId),
               }))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }

    private static void InternalLog<T>(this ILogger logger, IQuery<T> query, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = query.GetObjectSize();

        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("QuerySize", size),
                   new("Query", size > MaxSizeInBytes ? null : JsonSerializer.Serialize(query)),
                   new("QueryType", query.GetGenericTypeName()),
                   new("QueryId", query.QueryId),
               }))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }
}
