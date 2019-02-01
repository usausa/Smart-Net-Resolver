namespace ScopeBenchmark
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

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

        private ContextResolver childResolver;

        private ScopeResolver scopeResolver;

        [GlobalSetup]
        public void Setup()
        {
            resolver = new Resolver();
            childResolver = new ContextResolver(resolver, new ScopeStorage(), new object());
            scopeResolver = new ScopeResolver(new ScopeProvider());
        }

        [Benchmark]
        public object Resolver()
        {
            return resolver.GetInstance(RequestType);
        }

        [Benchmark]
        public object ContextResolver()
        {
            return childResolver.GetInstance(RequestType);
        }

        [Benchmark]
        public object ScopeResolver()
        {
            return scopeResolver.GetInstance(RequestType);
        }
    }

    public sealed class ScopeStorage
    {
        private readonly AsyncLocal<object> store = new AsyncLocal<object>();
        //private readonly ThreadLocal<object> store = new ThreadLocal<object>(); // faster than AsyncLocal

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

    public sealed class ContextResolverScope : IDisposable
    {
        private readonly ScopeStorage storage;

        private readonly object oldStore;

        public ContextResolverScope(ScopeStorage storage, object store)
        {
            this.storage = storage;
            oldStore = storage.Swap(store);
        }

        public void Dispose()
        {
            storage.Release(oldStore);
        }
    }

    public interface IScopeContainer
    {
        object GetOrCreate(Type type, Func<object> func);
    }

    public interface IScopeProvider
    {
        object GetInstance(IScopeContainer container, Type type);
    }

    public interface IResolver : IScopeContainer
    {
        object GetInstance(Type type);
    }

    public sealed class ScopeResolver : IResolver
    {
        private readonly ScopeProvider provider;

        private readonly Dictionary<Type, object> cache = new Dictionary<Type, object>();

        public ScopeResolver(ScopeProvider provider)
        {
            this.provider = provider;
        }

        public object GetOrCreate(Type type, Func<object> func)
        {
            lock (cache)
            {
                if (!cache.TryGetValue(type, out var target))
                {
                    target = func();
                    cache[type] = target;
                }

                return target;
            }
        }

        public object GetInstance(Type type)
        {
            return provider.GetInstance(this, type);
        }
    }

    public sealed class ContextResolver : IResolver
    {
        private readonly Resolver resolver;

        private readonly ScopeStorage storage;

        private readonly object store;

        public ContextResolver(Resolver resolver, ScopeStorage storage, object store)
        {
            this.resolver = resolver;
            this.storage = storage;
            this.store = store;
        }

        public object GetInstance(Type type)
        {
            using (new ContextResolverScope(storage, store))
            {
                return resolver.GetInstance(type);
            }
        }

        public object GetOrCreate(Type type, Func<object> func)
        {
            throw new NotSupportedException();
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

        public object GetOrCreate(Type type, Func<object> func)
        {
            throw new NotSupportedException();
        }
    }

    public sealed class ScopeProvider : IScopeProvider
    {
        private readonly Dictionary<Type, object> dummy = new Dictionary<Type, object>();

        private object Factory()
        {
            return new Data();
        }

        public object GetInstance(IScopeContainer container, Type type)
        {
            dummy.TryGetValue(type, out _);
            return container.GetOrCreate(type, Factory);
        }
    }

    public class Data
    {
    }
}
