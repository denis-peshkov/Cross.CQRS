namespace Cross.CQRS.Behaviors;

internal sealed class CommandEventQueueProcessBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly ICommandEventQueueReader _commandEventReader;
    private readonly IMediator _mediator;
    private readonly ILogger<CommandEventQueueProcessBehavior<TRequest, TResult>> _logger;

    public CommandEventQueueProcessBehavior(ICommandEventQueueReader commandEventReader, IMediator mediator, ILogger<CommandEventQueueProcessBehavior<TRequest, TResult>> logger)
    {
        _commandEventReader = commandEventReader;
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = await next();

            if (request is ICommand<TResult> identifiable)
            {
                await ProcessCommandEvents(identifiable.CommandId, CommandEventFlowTypeEnum.StandardFlow, cancellationToken);
            }

            return result;
        }
        finally
        {
            if (request is ICommand<TResult> identifiable)
            {
                await ProcessCommandEvents(identifiable.CommandId, CommandEventFlowTypeEnum.ExceptionSafeFlow, cancellationToken);
            }
        }
    }

    private async Task ProcessCommandEvents(Guid commandId, CommandEventFlowTypeEnum eventFlowType, CancellationToken cancellationToken)
    {
        var commandEvents = _commandEventReader.Read(commandId, eventFlowType);
        foreach (var commandEvent in commandEvents)
        {
            try
            {
                await _mediator.Publish((dynamic)commandEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while handling the CommandEvent {CommandEventType} for the CommandId {CommandId} with message: {ErrorMessage}",
                    commandEvent.GetType(),
                    commandId,
                    ex.Message);
            }
        }
    }
}
