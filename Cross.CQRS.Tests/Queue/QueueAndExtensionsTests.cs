namespace Cross.CQRS.Tests.Queue;

public class QueueAndExtensionsTests
{
    [Test]
    public void CommandEventQueue_Read_ReturnsOnlyMatchingEvents_AndKeepsOthers()
    {
        var queue = new CommandEventQueue();
        var targetId = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        queue.Writer.Write(new TestEvent(targetId, CommandEventFlowTypeEnum.StandardFlow));
        queue.Writer.Write(new TestEvent(targetId, CommandEventFlowTypeEnum.ExceptionSafeFlow));
        queue.Writer.Write(new TestEvent(otherId, CommandEventFlowTypeEnum.StandardFlow));

        var read = queue.Reader.Read(targetId, CommandEventFlowTypeEnum.StandardFlow);
        var rest = queue.Reader.Read(otherId, CommandEventFlowTypeEnum.StandardFlow);
        var safeForTarget = queue.Reader.Read(targetId, CommandEventFlowTypeEnum.ExceptionSafeFlow);

        read.Should().ContainSingle();
        rest.Should().ContainSingle();
        safeForTarget.Should().ContainSingle();
    }

    [Test]
    public void CommandEventQueue_Write_ThrowsOnNull()
    {
        var queue = new CommandEventQueue();
        var act = () => queue.Writer.Write(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GenericTypeExtensions_ReturnsExpectedNames()
    {
        typeof(string).GetGenericTypeName().Should().Be("String");
        typeof(List<int>).GetGenericTypeName().Should().Be("List<Int32>");
        new List<int>().GetGenericTypeName().Should().Be("List<Int32>");
    }

    [Test]
    public void ObjectExtensions_ReturnsSize_AndMaxValueForUnsupportedType()
    {
        var size = new { Name = "abc", Value = 12 }.GetObjectSize();
        var unsupported = new { Type = typeof(string) }.GetObjectSize();

        size.Should().BeGreaterThan(0);
        unsupported.Should().Be(int.MaxValue);
    }

    [Test]
    public void StopwatchHelper_ReturnsNonNegativeElapsedMilliseconds()
    {
        var start = Stopwatch.GetTimestamp();
        Thread.Sleep(5);
        var elapsed = StopwatchHelper.GetElapsedMilliseconds(start);

        elapsed.Should().BeGreaterThanOrEqualTo(0);
    }

    [Test]
    public void CommandAndQuery_HaveGeneratedIds()
    {
        var cmd = new TestCommand();
        var qry = new TestQuery();

        cmd.CommandId.Should().NotBe(Guid.Empty);
        qry.QueryId.Should().NotBe(Guid.Empty);
    }

    private sealed record TestEvent : CommandEvent
    {
        private readonly CommandEventFlowTypeEnum _flow;

        public TestEvent(Guid commandId, CommandEventFlowTypeEnum flow)
            : base(commandId)
        {
            _flow = flow;
        }

        public override CommandEventFlowTypeEnum EventFlowType() => _flow;
    }

    private sealed record TestCommand : Command
    {
    }

    private sealed record TestQuery : Cross.CQRS.Queries.Query<int>
    {
    }
}
