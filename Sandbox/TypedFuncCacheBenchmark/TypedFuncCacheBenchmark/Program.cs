using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Smart.Collections.Concurrent;

namespace TypedFuncCacheBenchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);
        }
    }

    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.Default, MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(Job.ShortRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly ObjectFuncCacheResolver objectFuncCacheResolver = new ObjectFuncCacheResolver();

        private readonly TypedFuncCacheResolver typedFuncCacheResolver = new TypedFuncCacheResolver();


        [GlobalSetup]
        public void Setup()
        {
            SetupObjectFuncCacheResolver();
            SetupTypedFuncCacheResolver();
        }

        private void SetupObjectFuncCacheResolver()
        {
            var singleton = (object)(new Singleton());
            var singletonFactory = (Func<object>)(() => singleton);
            var transientFactory = (Func<object>)(() => new Transient());
            var combineFactory = (Func<object>)(() => new Combine(
                (Singleton)singletonFactory(),
                (Transient)transientFactory()));
            var componentFactory = (Func<object>)(() => new Component((Singleton)singletonFactory()));
            var complexFactory = (Func<object>)(() => new Complex(
                (Singleton)singletonFactory(),
                (Singleton)singletonFactory(),
                (Component)componentFactory(),
                (Component)componentFactory(),
                (Transient)transientFactory(),
                (Transient)transientFactory()));

            objectFuncCacheResolver.Add(typeof(Singleton), singletonFactory);
            objectFuncCacheResolver.Add(typeof(Transient), transientFactory);
            objectFuncCacheResolver.Add(typeof(Combine), combineFactory);
            objectFuncCacheResolver.Add(typeof(Complex), complexFactory);
        }

        private void SetupTypedFuncCacheResolver()
        {
            var singleton = new Singleton();
            var singletonFactory = (Func<Singleton>)(() => singleton);
            var transientFactory = (Func<Transient>)(() => new Transient());
            var combineFactory = (Func<Combine>)(() => new Combine(
                singletonFactory(),
                transientFactory()));
            var componentFactory = (Func<Component>)(() => new Component(singletonFactory()));
            var complexFactory = (Func<Complex>)(() => new Complex(
                singletonFactory(),
                singletonFactory(),
                componentFactory(),
                componentFactory(),
                transientFactory(),
                transientFactory()));

            typedFuncCacheResolver.Add(typeof(Singleton), singletonFactory);
            typedFuncCacheResolver.Add(typeof(Transient), transientFactory);
            typedFuncCacheResolver.Add(typeof(Combine), combineFactory);
            typedFuncCacheResolver.Add(typeof(Complex), complexFactory);
        }

        [Benchmark]
        public Singleton SingletonFromObjectResolver() => objectFuncCacheResolver.Get<Singleton>();

        [Benchmark]
        public Transient TransientFromObjectResolver() => objectFuncCacheResolver.Get<Transient>();

        [Benchmark]
        public Combine CombineFromObjectResolver() => objectFuncCacheResolver.Get<Combine>();

        [Benchmark]
        public Complex ComplexFromObjectResolver() => objectFuncCacheResolver.Get<Complex>();

        [Benchmark]
        public Singleton SingletonFromTypedResolver() => typedFuncCacheResolver.Get<Singleton>();

        [Benchmark]
        public Transient TransientFromTypedResolver() => typedFuncCacheResolver.Get<Transient>();

        [Benchmark]
        public Combine CombineFromTypedResolver() => typedFuncCacheResolver.Get<Combine>();

        [Benchmark]
        public Complex ComplexFromTypedResolver() => typedFuncCacheResolver.Get<Complex>();
    }

    public sealed class ObjectFuncCacheResolver
    {
        private readonly ThreadsafeTypeHashArrayMap<Func<object>> cache = new ThreadsafeTypeHashArrayMap<Func<object>>();

        public void Add(Type type, Func<object> factory)
        {
            cache.AddIfNotExist(type, factory);
        }

        public T Get<T>()
        {
            if (cache.TryGetValue(typeof(T), out var func))
            {
                return (T)func();
            }

            return default;
        }

        public object Get(Type type)
        {
            if (cache.TryGetValue(type, out var func))
            {
                return func();
            }

            return default;
        }
    }

    public sealed class TypedFuncCacheResolver
    {
        private readonly ThreadsafeTypeHashArrayMap<object> cache = new ThreadsafeTypeHashArrayMap<object>();

        public void Add(Type type, object factory)
        {
            cache.AddIfNotExist(type, factory);
        }

        public T Get<T>()
        {
            if (cache.TryGetValue(typeof(T), out var func))
            {
                return ((Func<T>)func)();
            }

            return default;
        }

        public object Get(Type type)
        {
            if (cache.TryGetValue(type, out var func))
            {
                return ((Func<object>)func)();
            }

            return default;
        }
    }

    public class Singleton
    {
    }

    public class Transient
    {
    }

    public class Combine
    {
        public Singleton Singleton { get; }

        public Transient Transient { get; }

        public Combine(Singleton singleton, Transient transient)
        {
            Singleton = singleton;
            Transient = transient;
        }
    }

    public class Component
    {
        public Singleton Singleton { get; }

        public Component(Singleton singleton)
        {
            Singleton = singleton;
        }
    }

    public class Complex
    {
        public Singleton Singleton1 { get; }

        public Singleton Singleton2 { get; }

        public Component Component1 { get; }

        public Component Component2 { get; }

        public Transient Transient1 { get; }

        public Transient Transient2 { get; }

        public Complex(Singleton singleton1, Singleton singleton2, Component component1, Component component2, Transient transient1, Transient transient2)
        {
            Singleton1 = singleton1;
            Singleton2 = singleton2;
            Component1 = component1;
            Component2 = component2;
            Transient1 = transient1;
            Transient2 = transient2;
        }
    }
}
