namespace Cross.CQRS.Events;

public class CommandEventQueue : ICommandEventQueue
{
    private static readonly object _lock = new();

    public CommandEventQueue()
    {
        var store = new Queue<ICommandEvent>();

        Writer = new CommandEventQueueWriter(store);
        Reader = new CommandEventQueueReader(store);
    }

    /// <inheritdoc />
    public ICommandEventQueueWriter Writer { get; }

    /// <inheritdoc />
    public ICommandEventQueueReader Reader { get; }

    private sealed class CommandEventQueueReader : ICommandEventQueueReader
    {
        private readonly Queue<ICommandEvent> _eventStore;

        public CommandEventQueueReader(Queue<ICommandEvent> eventStore)
        {
            _eventStore = eventStore;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<ICommandEvent> Read(Guid requestId)
        {
            var result = new List<ICommandEvent>();
            var newQueue = new List<ICommandEvent>();

            lock (_lock)
            {
                while (_eventStore.TryDequeue(out var commandEvent))
                {
                    if (commandEvent.CommandId == requestId)
                    {
                        result.Add(commandEvent);
                    }
                    else
                    {
                        newQueue.Add(commandEvent);
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

    private sealed class CommandEventQueueWriter : ICommandEventQueueWriter
    {
        private readonly Queue<ICommandEvent> _eventStore;

        public CommandEventQueueWriter(Queue<ICommandEvent> eventStore)
        {
            _eventStore = eventStore;
        }

        /// <inheritdoc />
        public void Write(ICommandEvent commandEvent)
        {
            if (commandEvent == null)
            {
                throw new ArgumentNullException(nameof(commandEvent));
            }

            lock (_lock)
            {
                _eventStore.Enqueue(commandEvent);
            }
        }
    }
}
