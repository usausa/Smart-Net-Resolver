namespace Smart.Resolver.Mocks;

public sealed class ServiceWithConstructor
{
    public string Name { get; }

    public int Count { get; }

    public IService Service { get; }

    public ServiceWithConstructor(string name, int count, IService service)
    {
        Name = name;
        Count = count;
        Service = service;
    }
}
