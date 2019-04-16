namespace Smart.Resolver.Benchmark.Benchmarks
{
    using System;
    using System.Collections.Generic;

    using BenchmarkDotNet.Attributes;

    using Microsoft.Extensions.DependencyInjection;

    using Smart.Resolver;
    using Smart.Resolver.Benchmark.Classes;

    [Config(typeof(BenchmarkConfig))]
    public class SmartDefaultBenchmark
    {
        private SmartResolver resolver;

        private IServiceProvider provider;

        [GlobalSetup]
        public void Setup()
        {
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

            // ASP.NET Core simulation
            config.Bind<IServiceProvider>().To<SmartServiceProvider>().InSingletonScope();
            config.Bind<IServiceScopeFactory>().To<SmartServiceScopeFactory>().InSingletonScope();
            config.Bind<Controller>().ToSelf().InTransientScope();
            config.Bind<ITransientService1>().To<TransientService1>().InTransientScope();
            config.Bind<ITransientService2>().To<TransientService2>().InTransientScope();
            config.Bind<ITransientService3>().To<TransientService3>().InTransientScope();
            config.Bind<IScopedService>().To<ScopedService>().InContainerScope();

            resolver = config.ToResolver();

            Validator.Validate(t => resolver.Get(t));

            provider = resolver.Get<IServiceProvider>();
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Singleton()
        {
            resolver.Get(typeof(ISingleton1));
            resolver.Get(typeof(ISingleton2));
            resolver.Get(typeof(ISingleton3));
            resolver.Get(typeof(ISingleton4));
            resolver.Get(typeof(ISingleton5));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Transient()
        {
            resolver.Get(typeof(ITransient1));
            resolver.Get(typeof(ITransient2));
            resolver.Get(typeof(ITransient3));
            resolver.Get(typeof(ITransient4));
            resolver.Get(typeof(ITransient5));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Combined()
        {
            resolver.Get(typeof(Combined1));
            resolver.Get(typeof(Combined2));
            resolver.Get(typeof(Combined3));
            resolver.Get(typeof(Combined4));
            resolver.Get(typeof(Combined5));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Complex()
        {
            resolver.Get(typeof(Complex));
            resolver.Get(typeof(Complex));
            resolver.Get(typeof(Complex));
            resolver.Get(typeof(Complex));
            resolver.Get(typeof(Complex));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void Generics()
        {
            resolver.Get(typeof(IGenericObject<string>));
            resolver.Get(typeof(IGenericObject<int>));
            resolver.Get(typeof(IGenericObject<string>));
            resolver.Get(typeof(IGenericObject<int>));
            resolver.Get(typeof(IGenericObject<string>));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void MultipleSingleton()
        {
            resolver.Get(typeof(IEnumerable<IMultipleSingletonService>));
            resolver.Get(typeof(IEnumerable<IMultipleSingletonService>));
            resolver.Get(typeof(IEnumerable<IMultipleSingletonService>));
            resolver.Get(typeof(IEnumerable<IMultipleSingletonService>));
            resolver.Get(typeof(IEnumerable<IMultipleSingletonService>));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void MultipleTransient()
        {
            resolver.Get(typeof(IEnumerable<IMultipleTransientService>));
            resolver.Get(typeof(IEnumerable<IMultipleTransientService>));
            resolver.Get(typeof(IEnumerable<IMultipleTransientService>));
            resolver.Get(typeof(IEnumerable<IMultipleTransientService>));
            resolver.Get(typeof(IEnumerable<IMultipleTransientService>));
        }

        [Benchmark(OperationsPerInvoke = 5)]
        public void AspNet()
        {
            var factory = (IServiceScopeFactory)provider.GetService(typeof(IServiceScopeFactory));
            using (var scope = factory.CreateScope())
            {
                scope.ServiceProvider.GetService(typeof(Controller));
            }

            factory = (IServiceScopeFactory)provider.GetService(typeof(IServiceScopeFactory));
            using (var scope = factory.CreateScope())
            {
                scope.ServiceProvider.GetService(typeof(Controller));
            }

            factory = (IServiceScopeFactory)provider.GetService(typeof(IServiceScopeFactory));
            using (var scope = factory.CreateScope())
            {
                scope.ServiceProvider.GetService(typeof(Controller));
            }

            factory = (IServiceScopeFactory)provider.GetService(typeof(IServiceScopeFactory));
            using (var scope = factory.CreateScope())
            {
                scope.ServiceProvider.GetService(typeof(Controller));
            }

            factory = (IServiceScopeFactory)provider.GetService(typeof(IServiceScopeFactory));
            using (var scope = factory.CreateScope())
            {
                scope.ServiceProvider.GetService(typeof(Controller));
            }
        }
    }
}
