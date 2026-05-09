namespace Example.Configuration.Services;

#pragma warning disable CA1515
public sealed class MessageService
{
    private readonly IGreeter greeter;

    public MessageService(IGreeter greeter)
    {
        this.greeter = greeter;
    }

    public void PrintMessage(string name)
    {
        Console.WriteLine(greeter.Greet(name));
    }
}
