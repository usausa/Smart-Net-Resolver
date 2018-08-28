namespace Smart.Resolver.Benchmark.Benchmarks
{
    using BenchmarkDotNet.Attributes;

    using Smart.Reflection;
    using Smart.Resolver;
    using Smart.Resolver.Benchmark.Classes;

    [Config(typeof(BenchmarkConfig))]
    public class SmartUseReflectionBenchmark
    {
        private SmartResolver resolver;

        [GlobalSetup]
        public void Setup()
        {
            DelegateFactory.Default.ConfigureSafe();
            var config = new ResolverConfig();
            config.UseOpenGenericBinding();
            config.UseArrayBinding();

            config.Bind<ISingleton1>().To<Singleton1>().InSingletonScope();
            config.Bind<ISingleton2>().To<Singleton2>().InSingletonScope();
            config.Bind<ISingleton3>().To<Singleton3>().InSingletonScope();
            config.Bind<ISingleton4>().To<Singleton4>().InSingletonScope();
            config.Bind<ISingleton5>().To<Singleton5>().InSingletonScope();
            config.Bind<ITransient1>().To<Transient1>().InTransientScope();
            config.Bind<ITransient2>().To<Transient2>().InTransientScope();
            config.Bind<ITransient3>().To<Transient3>().InTransientScope();
            config.Bind<ITransient4>().To<Transient4>().InTransientScope();
            config.Bind<ITransient5>().To<Transient5>().InTransientScope();
            config.Bind<Combined1>().ToSelf().InTransientScope();
            config.Bind<Combined2>().ToSelf().InTransientScope();
            config.Bind<Combined3>().ToSelf().InTransientScope();
            config.Bind<Combined4>().ToSelf().InTransientScope();
            config.Bind<Combined5>().ToSelf().InTransientScope();
            config.Bind<Complex>().ToSelf().InTransientScope();
            config.Bind(typeof(IGenericObject<>)).To(typeof(GenericObject<>)).InTransientScope();
            config.Bind<IMultipleSingletonService>().To<MultipleSingletonService1>().InSingletonScope();
            config.Bind<IMultipleSingletonService>().To<MultipleSingletonService2>().InSingletonScope();
            config.Bind<IMultipleSingletonService>().To<MultipleSingletonService3>().InSingletonScope();
            config.Bind<IMultipleSingletonService>().To<MultipleSingletonService4>().InSingletonScope();
            config.Bind<IMultipleSingletonService>().To<MultipleSingletonService5>().InSingletonScope();
            config.Bind<IMultipleTransientService>().To<MultipleTransientService1>().InTransientScope();
            config.Bind<IMultipleTransientService>().To<MultipleTransientService2>().InTransientScope();
            config.Bind<IMultipleTransientService>().To<MultipleTransientService3>().InTransientScope();
            config.Bind<IMultipleTransientService>().To<MultipleTransientService4>().InTransientScope();
            config.Bind<IMultipleTransientService>().To<MultipleTransientService5>().InTransientScope();

            resolver = config.ToResolver();

            Validator.Validate(t => resolver.Get(t));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Singleton()
        {
            resolver.Get(RequestTypes.Singleton1);
            resolver.Get(RequestTypes.Singleton2);
            resolver.Get(RequestTypes.Singleton3);
            resolver.Get(RequestTypes.Singleton4);
            resolver.Get(RequestTypes.Singleton5);
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Transient()
        {
            resolver.Get(RequestTypes.Transient1);
            resolver.Get(RequestTypes.Transient2);
            resolver.Get(RequestTypes.Transient3);
            resolver.Get(RequestTypes.Transient4);
            resolver.Get(RequestTypes.Transient5);
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Combined()
        {
            resolver.Get(RequestTypes.Combined1);
            resolver.Get(RequestTypes.Combined2);
            resolver.Get(RequestTypes.Combined3);
            resolver.Get(RequestTypes.Combined4);
            resolver.Get(RequestTypes.Combined5);
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Complex()
        {
            resolver.Get(RequestTypes.Complex);
            resolver.Get(RequestTypes.Complex);
            resolver.Get(RequestTypes.Complex);
            resolver.Get(RequestTypes.Complex);
            resolver.Get(RequestTypes.Complex);
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Generics()
        {
            resolver.Get(RequestTypes.Generic1);
            resolver.Get(RequestTypes.Generic2);
            resolver.Get(RequestTypes.Generic1);
            resolver.Get(RequestTypes.Generic2);
            resolver.Get(RequestTypes.Generic1);
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void MultipleSingleton()
        {
            resolver.Get(RequestTypes.MultipleSingleton);
            resolver.Get(RequestTypes.MultipleSingleton);
            resolver.Get(RequestTypes.MultipleSingleton);
            resolver.Get(RequestTypes.MultipleSingleton);
            resolver.Get(RequestTypes.MultipleSingleton);
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void MultipleTransient()
        {
            resolver.Get(RequestTypes.MultipleTransient);
            resolver.Get(RequestTypes.MultipleTransient);
            resolver.Get(RequestTypes.MultipleTransient);
            resolver.Get(RequestTypes.MultipleTransient);
            resolver.Get(RequestTypes.MultipleTransient);
        }
    }
}
