namespace FactoryBenchmark
{
    public class Service1
    {
    }

    public class Service2
    {
    }

    public class Usecase1
    {
        public Service1 Service1 { get; }

        public Usecase1(Service1 service1)
        {
            Service1 = service1;
        }
    }

    public class Usecase2
    {
        public Service2 Service2 { get; }

        public Usecase2(Service2 service2)
        {
            Service2 = service2;
        }
    }

    public class Transient1
    {
    }

    public class Transient2
    {
    }


    public class Complex
    {
        public Service1 Service1 { get; }

        public Service2 Service2 { get; }

        public Usecase1 Usecase1 { get; }

        public Usecase2 Usecase2 { get; }

        public Transient1 Transient1 { get; }

        public Transient2 Transient2 { get; }

        public Complex(
            Service1 service1,
            Service2 service2,
            Usecase1 usecase1,
            Usecase2 usecase2,
            Transient1 transient1,
            Transient2 transient2)
        {
            Service1 = service1;
            Service2 = service2;
            Usecase1 = usecase1;
            Usecase2 = usecase2;
            Transient1 = transient1;
            Transient2 = transient2;
        }
    }
}
