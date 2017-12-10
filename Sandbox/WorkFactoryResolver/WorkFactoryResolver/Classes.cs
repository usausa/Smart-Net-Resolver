namespace WorkFactoryResolver
{
    public class Service1
    {
    }

    public class Service2
    {
    }

    public class Service3
    {
    }

    public class SubObject1
    {
    }

    public class SubObject2
    {
    }

    public class SubObject3
    {
    }

    public class Complex
    {
        public Service1 Service1 { get; }

        public Service2 Service2 { get; }

        public Service3 Service3 { get; }

        public SubObject1 SubObject1 { get; }

        public SubObject2 SubObject2 { get; }

        public SubObject3 SubObject3 { get; }

        public Complex(
            Service1 service1,
            Service2 service2,
            Service3 service3,
            SubObject1 subObject1,
            SubObject2 subObject2,
            SubObject3 subObject3)
        {
            Service1 = service1;
            Service2 = service2;
            Service3 = service3;
            SubObject1 = subObject1;
            SubObject2 = subObject2;
            SubObject3 = subObject3;
        }
    }
}
