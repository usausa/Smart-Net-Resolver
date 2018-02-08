namespace PerformanceBenchmark.Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Jobs;

    using PerformanceBenchmark.Classes;

    using Smart.Resolver;

    [Config(typeof(BenchmarkConfig))]
    public class SmartDefaultBenchmark
    {
        private SmartResolver resolver;

        [GlobalSetup]
        public void Setup()
        {
            var config = new ResolverConfig();
            config.UseOpenGenericBinding();
            config.UseArrayBinding();

            config.Bind<ISingleton1>().To<Singleton1>().InSingletonScope();
            config.Bind<ISingleton2>().To<Singleton2>().InSingletonScope();
            config.Bind<ISingleton3>().To<Singleton3>().InSingletonScope();
            config.Bind<ITransient1>().To<Transient1>().InTransientScope();
            config.Bind<ITransient2>().To<Transient2>().InTransientScope();
            config.Bind<ITransient3>().To<Transient3>().InTransientScope();
            config.Bind<Combined1>().ToSelf().InTransientScope();
            config.Bind<Combined2>().ToSelf().InTransientScope();
            config.Bind<Combined3>().ToSelf().InTransientScope();
            config.Bind(typeof(IGenericObject<>)).To(typeof(GenericObject<>)).InTransientScope();
            config.Bind<IMultpleSingletonService>().To<MultpleSingletonService1>().InSingletonScope();
            config.Bind<IMultpleSingletonService>().To<MultpleSingletonService2>().InSingletonScope();
            config.Bind<IMultpleSingletonService>().To<MultpleSingletonService3>().InSingletonScope();
            config.Bind<IMultpleSingletonService>().To<MultpleSingletonService4>().InSingletonScope();
            config.Bind<IMultpleSingletonService>().To<MultpleSingletonService5>().InSingletonScope();
            config.Bind<IMultpleTransientService>().To<MultpleTransientService1>().InTransientScope();
            config.Bind<IMultpleTransientService>().To<MultpleTransientService2>().InTransientScope();
            config.Bind<IMultpleTransientService>().To<MultpleTransientService3>().InTransientScope();
            config.Bind<IMultpleTransientService>().To<MultpleTransientService4>().InTransientScope();
            config.Bind<IMultpleTransientService>().To<MultpleTransientService5>().InTransientScope();

            resolver = config.ToResolver();

            Validator.Validate(t => resolver.Get(t));
        }

        [Benchmark]
        public void Singleton()
        {
            resolver.Get(RequestTypes.Singleton1);
            resolver.Get(RequestTypes.Singleton2);
            resolver.Get(RequestTypes.Singleton3);
        }

        [Benchmark]
        public void Transient()
        {
            resolver.Get(RequestTypes.Transient1);
            resolver.Get(RequestTypes.Transient2);
            resolver.Get(RequestTypes.Transient3);
        }

        [Benchmark]
        public void Combined()
        {
            resolver.Get(RequestTypes.Combined1);
            resolver.Get(RequestTypes.Combined2);
            resolver.Get(RequestTypes.Combined3);
        }

        [Benchmark]
        public void Generics()
        {
            resolver.Get(RequestTypes.Generic1);
        }

        [Benchmark]
        public void MultipleSingleton()
        {
            resolver.Get(RequestTypes.MultipleSinglton);
        }

        [Benchmark]
        public void MultipleTransient()
        {
            resolver.Get(RequestTypes.MultipleTransient);
        }
    }
}
