namespace NewFactoryWork
{
    public class Data2
    {
        public string Parameter1 { get; }

        public int Parameter2 { get; }

        public Data2(string p1, int p2)
        {
            Parameter1 = p1;
            Parameter2 = p2;
        }
    }

    public class Data : IInitialize
    {
        public void Initialize()
        {
        }
    }

    public interface ISingleton
    {
        void DoSomething();
    }

    public class Singleton : ISingleton
    {
        public void DoSomething()
        {
        }
    }

    public interface ITransient
    {
        void DoSomething();
    }

    public class Transient : ITransient
    {
        public void DoSomething()
        {
        }
    }

    public class Combined
    {
        public ISingleton Singleton { get; }

        public Combined(ISingleton singleton)
        {
            Singleton = singleton;
        }
    }

    public class Complex
    {
        public ITransient Transient { get; }

        public Combined Combined { get; }

        public Complex(
            ITransient transient,
            Combined combined)
        {
            Transient = transient;
            Combined = combined;
        }
    }

    public interface IService
    {
    }

    public sealed class Service1 : IService
    {
    }

    public sealed class Service2 : IService
    {
    }

    public sealed class Service3 : IService
    {
    }
}
