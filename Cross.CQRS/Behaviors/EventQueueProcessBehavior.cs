namespace Cross.CQRS.Behaviors;

internal sealed class EventQueueProcessBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IEventQueueReader _eventReader;
    private readonly IMediator _mediator;

    public EventQueueProcessBehavior(IEventQueueReader eventReader, IMediator mediator)
    {
        _eventReader = eventReader;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (request is ICommand<TResult> identifiable)
        {
            var events = _eventReader.Read(identifiable.CommandId);
            foreach (var @event in events)
            {
                await _mediator.Publish((dynamic)@event, cancellationToken);
            }
        }

        return result;
    }
}
