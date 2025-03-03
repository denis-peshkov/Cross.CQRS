namespace Cross.CQRS.Events;

public enum CommandEventTypeEnum
{
    InTransaction = 1,

    OutOfTransaction = 2,
}