namespace NewFactoryWork
{
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
}
