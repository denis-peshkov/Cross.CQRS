namespace Cross.CQRS.Events;

public class EventQueue : IEventQueue
{
    private static readonly object _lock = new();

    public EventQueue()
    {
        var store = new Queue<IEvent>();

        Writer = new EventQueueWriter(store);
        Reader = new EventQueueReader(store);
    }

    /// <inheritdoc />
    public IEventQueueWriter Writer { get; }

    /// <inheritdoc />
    public IEventQueueReader Reader { get; }

    private sealed class EventQueueReader : IEventQueueReader
    {
        private readonly Queue<IEvent> _eventStore;

        public EventQueueReader(Queue<IEvent> eventStore)
        {
            _eventStore = eventStore;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IEvent> Read(Guid requestId)
        {
            var result = new List<IEvent>();
            var newQueue = new List<IEvent>();

            lock (_lock)
            {
                while (_eventStore.TryDequeue(out var @event))
                {
                    if (@event.CommandId == requestId)
                    {
                        result.Add(@event);
                    }
                    else
                    {
                        newQueue.Add(@event);
                    }
                }

                _eventStore.Clear();
                newQueue.Reverse(); // Reverse to preserve order

                foreach (var unmatched in newQueue)
                {
                    _eventStore.Enqueue(unmatched);
                }

                return result;
            }
        }
    }

    private sealed class EventQueueWriter : IEventQueueWriter
    {
        private readonly Queue<IEvent> _eventStore;

        public EventQueueWriter(Queue<IEvent> eventStore)
        {
            _eventStore = eventStore;
        }

        /// <inheritdoc />
        public void Write(IEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            lock (_lock)
            {
                _eventStore.Enqueue(@event);
            }
        }
    }
}
