namespace Smart.Resolver.Mocks
{
    public class Controller
    {
        public IService Service { get; }

        public Controller(IService service)
        {
            Service = service;
        }
    }
}
