using System;

namespace WorkFactoryResolver
{
    using BenchmarkDotNet.Attributes;

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly ObjectResolver objectResolver = new ObjectResolver();

        private readonly FactoryResolver factoryResolver = new FactoryResolver();

        private readonly Type resolveType = typeof(Complex);

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

            resolver.Get(resolveType);
        }

        [Benchmark]
        public object ObjectResolver()
        {
            return objectResolver.Get(resolveType);
        }

        [Benchmark]
        public object FactoryResolver()
        {
            return factoryResolver.Get(resolveType);
        }
    }
}
