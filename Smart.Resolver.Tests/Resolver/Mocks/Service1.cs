namespace Smart.Resolver.Mocks;

public sealed class Service1 : IService
{
    public bool Executed { get; private set; }

    public void Execute()
    {
        Executed = true;
    }
}
