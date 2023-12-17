namespace Smart.Resolver.Mocks;

public sealed class Controller
{
    public IService Service { get; }

    public Controller(IService service)
    {
        Service = service;
    }
}
