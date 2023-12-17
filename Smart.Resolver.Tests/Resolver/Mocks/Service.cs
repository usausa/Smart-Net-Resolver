namespace Smart.Resolver.Mocks;

public sealed class Service : IService
{
    public bool Executed { get; private set; }

    public void Execute()
    {
        Executed = true;
    }
}
