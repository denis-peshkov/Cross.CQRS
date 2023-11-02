namespace Cross.CQRS.Behaviors;

internal sealed class EventQueueProcessBehavior<T, TR> : IPipelineBehavior<T, TR> where T : IRequest<TR>
{
    private readonly IEventQueueReader _eventReader;
    private readonly IMediator _mediator;

    public EventQueueProcessBehavior(IEventQueueReader eventReader, IMediator mediator)
    {
        _eventReader = eventReader;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<TR> Handle(T request, RequestHandlerDelegate<TR> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (request is ICommand<TR> identifiable)
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
