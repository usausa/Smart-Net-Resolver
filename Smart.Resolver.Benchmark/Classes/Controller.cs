namespace Smart.Resolver.Benchmark.Classes
{
    public class Controller
    {
        public ITransientService1 TransientService1 { get; }

        public ITransientService2 TransientService2 { get; }

        public ITransientService3 TransientService3 { get; }

        public Controller(ITransientService1 transientService1, ITransientService2 transientService2, ITransientService3 transientService3)
        {
            TransientService1 = transientService1;
            TransientService2 = transientService2;
            TransientService3 = transientService3;
        }
    }
}
