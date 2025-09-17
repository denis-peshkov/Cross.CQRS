namespace SampleWebApp.Modules.TestEventsCommand.Events;

public class TestOnExceptionOverriddenEventHandler : CommandEventHandler<TestOnExceptionOverriddenEvent>
{
    public TestOnExceptionOverriddenEventHandler(ILogger<TestOnExceptionOverriddenEventHandler> logger)
        : base(logger)
    {
    }

    protected override async Task HandleAsync(TestOnExceptionOverriddenEvent @event, CancellationToken cancellationToken)
    {
        // Коротко: хэндлер срабатывает дважды из-за контравариантности INotificationHandler<in T>.
        // Когда ты публикуешь экземпляр производного события (TestOnExceptionOverriddenEvent), MediatR вызывает все подходящие хэндлеры:
        // 	• INotificationHandler<TestOnExceptionOverriddenEvent> (точное совпадение типа)
        // 	• и также INotificationHandler<TestOnExceptionEvent> (базовый тип подходит благодаря in — контравариантность)
        //
        // Поэтому, если у тебя есть хэндлер на базовый TestOnExceptionEvent, он тоже будет вызван для наследника — получаешь два вызова.
        //
        // Это поведение усиливает ещё и твой Publish((dynamic)commandEvent): рантайм-тип берётся как TestOnExceptionOverriddenEvent, и MediatR собирает
        // всех обработчиков, совместимых с этим типом (включая обработчики базовых типов).

        Logger.LogInformation("TestOnExceptionOverriddenEventHandler");
    }
}
