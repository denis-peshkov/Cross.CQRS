namespace SampleWebApp.Modules.TestCommandGeneric.Handlers;

public class TestGenericCommand : Command<int>
{
    public string Text { get; set; }

    public TestGenericCommand(string text)
    {
        Text = text;
    }
}
