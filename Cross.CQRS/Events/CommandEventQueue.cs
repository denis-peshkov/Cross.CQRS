namespace Cross.CQRS.Events;

public sealed class CommandEventQueue : ICommandEventQueue
{
    private readonly object _lock = new();

    public CommandEventQueue()
    {
        var store = new Queue<ICommandEvent>();

        Writer = new CommandEventQueueWriter(store, _lock);
        Reader = new CommandEventQueueReader(store, _lock);
    }

    /// <inheritdoc />
    public ICommandEventQueueWriter Writer { get; }

    /// <inheritdoc />
    public ICommandEventQueueReader Reader { get; }

    private sealed class CommandEventQueueReader : ICommandEventQueueReader
    {
        private readonly Queue<ICommandEvent> _eventStore;
        private readonly object _lock;

        public CommandEventQueueReader(Queue<ICommandEvent> eventStore, object lck)
        {
            _eventStore = eventStore;
            _lock = lck;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<ICommandEvent> Read(Guid requestId, CommandEventFlowTypeEnum commandEventFlow)
        {
            var result = new Queue<ICommandEvent>();
            var newQueue = new Queue<ICommandEvent>();

            lock (_lock)
            {
                while (_eventStore.TryDequeue(out var commandEvent))
                {
                    if (commandEvent.CommandId == requestId && commandEvent.EventFlowType() == commandEventFlow)
                    {
                        result.Enqueue(commandEvent);
                    }
                    else
                    {
                        newQueue.Enqueue(commandEvent);
                    }
                }

                // возвращаем НЕподходящие в исходном порядке (без Reverse!)
                while (newQueue.TryDequeue(out var ev))
                    _eventStore.Enqueue(ev);

                return result;
            }
        }
    }

    private sealed class CommandEventQueueWriter : ICommandEventQueueWriter
    {
        private readonly Queue<ICommandEvent> _eventStore;
        private readonly object _lock;

        public CommandEventQueueWriter(Queue<ICommandEvent> eventStore, object lck)
        {
            _eventStore = eventStore;
            _lock = lck;
        }

        /// <inheritdoc />
        public void Write(ICommandEvent commandEvent)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(commandEvent);
#else
            if (commandEvent == null)
            {
                throw new ArgumentNullException(nameof(commandEvent));
            }
#endif
            lock (_lock)
            {
                _eventStore.Enqueue(commandEvent);
            }
        }
    }
}
