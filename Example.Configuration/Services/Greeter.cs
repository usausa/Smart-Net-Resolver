namespace Example.Configuration.Services;

#pragma warning disable CA1515
public interface IGreeter
{
    string Greet(string name);
}

public sealed class HelloGreeter : IGreeter
{
    public string Greet(string name) => $"Hello, {name}!";
}

public sealed class HiGreeter : IGreeter
{
    public string Greet(string name) => $"Hi, {name}!";
}
