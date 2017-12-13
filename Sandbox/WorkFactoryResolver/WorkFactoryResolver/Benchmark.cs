using System;

namespace WorkFactoryResolver
{
    using BenchmarkDotNet.Attributes;

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly ObjectResolver objectResolver = new ObjectResolver();

        private readonly FactoryResolver factoryResolver = new FactoryResolver();

        private readonly Type singletonType = typeof(Service1);

        private readonly Type transientType = typeof(SubObject1);

        private readonly Type complexType = typeof(Complex);

        [GlobalSetup]
        public void Setup()
        {
            Prepare(objectResolver);
            Prepare(factoryResolver);
        }

        private void Prepare(IResolver resolver)
        {
            resolver.RegisterSingleton(typeof(Service1));
            resolver.RegisterSingleton(typeof(Service2));
            resolver.RegisterSingleton(typeof(Service3));
            resolver.RegisterTransient(typeof(SubObject1));
            resolver.RegisterTransient(typeof(SubObject2));
            resolver.RegisterTransient(typeof(SubObject3));
            resolver.RegisterTransient(typeof(Complex));

            resolver.Get(complexType);
        }

        [Benchmark]
        public object ObjectResolverSingleton()
        {
            return objectResolver.Get(singletonType);
        }

        [Benchmark]
        public object FactoryResolverSingleton()
        {
            return factoryResolver.Get(singletonType);
        }

        [Benchmark]
        public object ObjectResolverTransient()
        {
            return objectResolver.Get(transientType);
        }

        [Benchmark]
        public object FactoryResolverTransient()
        {
            return factoryResolver.Get(transientType);
        }

        [Benchmark]
        public object ObjectResolverComplex()
        {
            return objectResolver.Get(complexType);
        }

        [Benchmark]
        public object FactoryResolverComplex()
        {
            return factoryResolver.Get(complexType);
        }
    }
}
