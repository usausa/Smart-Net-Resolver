namespace Smart.Resolver.Benchmark.Classes
{
    public interface IMultipleSingletonService
    {
        void DoSomething();
    }

    public class MultipleSingletonService1 : IMultipleSingletonService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleSingletonService2 : IMultipleSingletonService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleSingletonService3 : IMultipleSingletonService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleSingletonService4 : IMultipleSingletonService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleSingletonService5 : IMultipleSingletonService
    {
        public void DoSomething()
        {
        }
    }

    public interface IMultipleTransientService
    {
        void DoSomething();
    }

    public class MultipleTransientService1 : IMultipleTransientService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleTransientService2 : IMultipleTransientService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleTransientService3 : IMultipleTransientService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleTransientService4 : IMultipleTransientService
    {
        public void DoSomething()
        {
        }
    }

    public class MultipleTransientService5 : IMultipleTransientService
    {
        public void DoSomething()
        {
        }
    }
}
