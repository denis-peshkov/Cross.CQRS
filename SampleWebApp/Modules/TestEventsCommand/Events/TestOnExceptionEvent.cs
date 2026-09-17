namespace SampleWebApp.Modules.TestEventsCommand.Events;

public record TestOnExceptionEvent : CommandEvent
{
    public TestOnExceptionEvent(Guid commandId)
        : base(commandId)
    {
    }

    public override CommandEventFlowTypeEnum EventFlowType()
        => CommandEventFlowTypeEnum.ExceptionSafeFlow;
}
