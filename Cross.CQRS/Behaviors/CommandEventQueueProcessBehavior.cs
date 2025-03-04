namespace Cross.CQRS.Behaviors;

internal sealed class CommandEventQueueProcessBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly ICommandEventQueueReader _commandEventReader;
    private readonly IMediator _mediator;

    public CommandEventQueueProcessBehavior(ICommandEventQueueReader commandEventReader, IMediator mediator)
    {
        _commandEventReader = commandEventReader;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = await next();

            if (request is ICommand<TResult> identifiable)
            {
                var commandEvents = _commandEventReader.Read(identifiable.CommandId);
                foreach (var commandEvent in commandEvents.Where(x => x.EventFlowType() == CommandEventFlowTypeEnum.OnStandardFlow))
                {
                    await _mediator.Publish((dynamic)commandEvent, cancellationToken);
                }
            }

            return result;
        }
        finally
        {
            if (request is ICommand<TResult> identifiable)
            {
                var commandEvents = _commandEventReader.Read(identifiable.CommandId);
                foreach (var commandEvent in commandEvents.Where(x => x.EventFlowType() == CommandEventFlowTypeEnum.OnExceptionalFlow))
                {
                    await _mediator.Publish((dynamic)commandEvent, cancellationToken);
                }
            }
        }
    }
}
