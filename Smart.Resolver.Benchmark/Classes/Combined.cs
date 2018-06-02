namespace Smart.Resolver.Benchmark.Classes
{
    public class Combined1
    {
        public ISingleton1 Singleton { get; }

        public Combined1(ISingleton1 singleton)
        {
            Singleton = singleton;
        }

        public void DoSomething()
        {
        }
    }

    public class Combined2
    {
        public ISingleton2 Singleton { get; }

        public Combined2(ISingleton2 singleton)
        {
            Singleton = singleton;
        }

        public void DoSomething()
        {
        }
    }

    public class Combined3
    {
        public ISingleton3 Singleton { get; }

        public Combined3(ISingleton3 singleton)
        {
            Singleton = singleton;
        }

        public void DoSomething()
        {
        }
    }
}
