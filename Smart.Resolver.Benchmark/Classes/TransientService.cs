namespace Smart.Resolver.Benchmark.Classes
{
    public interface ITransientService1
    {
        void DoSomething();
    }

    public class TransientService1 : ITransientService1
    {
        public IScopedService ScopedService { get; }

        public TransientService1(IScopedService scopedService)
        {
            ScopedService = scopedService;
        }

        public void DoSomething()
        {
        }
    }

    public interface ITransientService2
    {
        void DoSomething();
    }

    public class TransientService2 : ITransientService2
    {
        public IScopedService ScopedService { get; }

        public TransientService2(IScopedService scopedService)
        {
            ScopedService = scopedService;
        }

        public void DoSomething()
        {
        }
    }

    public interface ITransientService3
    {
        void DoSomething();
    }

    public class TransientService3 : ITransientService3
    {
        public IScopedService ScopedService { get; }

        public TransientService3(IScopedService scopedService)
        {
            ScopedService = scopedService;
        }

        public void DoSomething()
        {
        }
    }
}
