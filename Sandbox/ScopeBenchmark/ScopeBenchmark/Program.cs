using System.Collections.Generic;
using System.Threading;

namespace ScopeBenchmark
{
    using System;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;

    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
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

    // Resolver    : 10
    // AsyncLocal  : 90(58) + 32 : + 80
    // ThreadLocal : 60(49) + 11 : + 50

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private static readonly Type RequestType = typeof(object);

        private Resolver resolver;

        private ChildResolver childResolver;

        [GlobalSetup]
        public void Setup()
        {
            resolver = new Resolver();
            childResolver = new ChildResolver(resolver, new ScopeStorage(), new object());
        }

        [Benchmark]
        public object Resolver()
        {
            return resolver.GetInstance(RequestType);
        }

        [Benchmark]
        public object ChildResolver()
        {
            return childResolver.GetInstance(RequestType);
        }
    }

    public sealed class ScopeStorage
    {
        //private readonly AsyncLocal<object> store = new AsyncLocal<object>();
        private readonly ThreadLocal<object> store = new ThreadLocal<object>(); // faster than AsyncLocal

        public object Store => store;

        public object Swap(object newStore)
        {
            var result = store.Value;
            store.Value = newStore;   // Slow
            return result;
        }

        public void Release(object oldStore)
        {
            store.Value = oldStore;
        }
    }

    public sealed class ChildResolverScope : IDisposable
    {
        private readonly ScopeStorage storage;

        private readonly object oldStore;

        public ChildResolverScope(ScopeStorage storage, object store)
        {
            this.storage = storage;
            oldStore = storage.Swap(store);
        }

        public void Dispose()
        {
            storage.Release(oldStore);
        }
    }

    public interface IResolver
    {
        object GetInstance(Type type);
    }

    public sealed class ChildResolver : IResolver
    {
        private readonly Resolver resolver;

        private readonly ScopeStorage storage;

        private readonly object store;

        public ChildResolver(Resolver resolver, ScopeStorage storage, object store)
        {
            this.resolver = resolver;
            this.storage = storage;
            this.store = store;
        }

        public object GetInstance(Type type)
        {
            using (new ChildResolverScope(storage, store))
            {
                return resolver.GetInstance(type);
            }
        }
    }

    public sealed class Resolver : IResolver
    {
        private readonly Dictionary<Type, object> dummy = new Dictionary<Type, object>();

        public object GetInstance(Type type)
        {
            dummy.TryGetValue(type, out _);
            return new Data();
        }
    }

    public class Data
    {
    }
}
