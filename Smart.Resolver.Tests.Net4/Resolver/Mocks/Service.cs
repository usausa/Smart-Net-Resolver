namespace Smart.Resolver.Mocks
{
    public class Service : IService
    {
        public bool Executed { get; private set; }

        public void Execute()
        {
            Executed = true;
        }
    }
}
