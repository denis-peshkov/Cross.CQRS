namespace Cross.CQRS.Events;

public enum CommandEventFlowTypeEnum
{
    /// <summary>
    /// Processes events only when the command executes successfully without errors.
    /// Used for the standard event processing flow.
    /// </summary>
    StandardFlow = 1,

    /// <summary>
    /// Ensures the event is processed even if an exception occurs during command execution.
    /// Used for a safe error-handling flow.
    /// </summary>
    ExceptionSafeFlow = 2,
}
