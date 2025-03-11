namespace Cross.CQRS.Extensions;

public static class InternalLogExtensions
{
    private static int MaxSizeInBytes => 200 * 1024;

    public static void InternalLogTrace(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Trace, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Trace, null, message, args);
                break;
        }
    }

    public static void InternalLogTrace(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Trace, ex, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Trace, ex, message, args);
                break;
        }
    }

    public static void IntegrationLogDebug(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Debug, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Debug, null, message, args);
                break;
        }
    }

    public static void IntegrationLogDebug(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Debug, ex, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Debug, ex, message, args);
                break;
        }
    }

    public static void IntegrationLogInformation(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Information, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Information, null, message, args);
                break;
        }
    }

    public static void IntegrationLogInformation(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Information, ex, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Information, ex, message, args);
                break;
        }
    }

    public static void IntegrationLogWarning(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Warning, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Warning, null, message, args);
                break;
        }
    }

    public static void IntegrationLogWarning(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Warning, ex, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Warning, ex, message, args);
                break;
        }
    }

    public static void IntegrationLogError(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Error, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Error, null, message, args);
                break;
        }
    }

    public static void IntegrationLogError(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Error, ex, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Error, ex, message, args);
                break;
        }
    }

    public static void IntegrationLogCritical(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Critical, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Critical, null, message, args);
                break;
        }
    }

    public static void IntegrationLogCritical(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, LogLevel.Critical, ex, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, LogLevel.Critical, ex, message, args);
                break;
        }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private static void IntegrationLog(this ILogger logger, ICommandOrCommandEvent commandOrCommandEvent, LogLevel logLevel, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        switch (commandOrCommandEvent)
        {
            case ICommandEvent commandEvent:
                logger.IntegrationLog(commandEvent, logLevel, null, message, args);
                break;
            case ICommand command:
                logger.IntegrationLog(command, logLevel, null, message, args);
                break;
        }
    }

    private static void IntegrationLog(this ILogger logger, ICommandEvent ev, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = ev.GetObjectSize();

        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("CommandEventSize", size),
                   new("CommandEvent", size > MaxSizeInBytes ? null : ev),
                   new("CommandEventType", ev.GetGenericTypeName()),
                   new("CommandId", ev.CommandId),
               }))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }

    private static void IntegrationLog(this ILogger logger, ICommand command, LogLevel logLevel, Exception? ex, [StructuredMessageTemplate] string? message, params object?[] args)
    {
        var size = command.GetObjectSize();

        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("CommandSize", size),
                   new("Command", size > MaxSizeInBytes ? null : command),
                   new("CommandType", command.GetGenericTypeName()),
                   new("CommandId", command.CommandId),
               }))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel, ex, message, args);
        }
    }
}
