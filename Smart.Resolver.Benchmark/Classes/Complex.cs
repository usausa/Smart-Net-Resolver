namespace Smart.Resolver.Benchmark.Classes
{
    public class Complex
    {
        public ISingleton1 Singleton1 { get; }

        public ISingleton2 Singleton2 { get; }

        public ISingleton3 Singleton3 { get; }

        public Combined1 Combined1 { get; }

        public Combined2 Combined2 { get; }

        public Combined3 Combined3 { get; }

        public Complex(
            ISingleton1 singleton1,
            ISingleton2 singleton2,
            ISingleton3 singleton3,
            Combined1 combined1,
            Combined2 combined2,
            Combined3 combined3)
        {
            Singleton1 = singleton1;
            Singleton2 = singleton2;
            Singleton3 = singleton3;
            Combined1 = combined1;
            Combined2 = combined2;
            Combined3 = combined3;
        }
    }
}
