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
    // Context     : 30
    // Child       : 50

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private static readonly Type RequestType = typeof(Data);

        private Resolver resolver;

        private ContextScopeResolver contextScopeResolver;

        private ChildScopeResolver childScopeResolver;

        [GlobalSetup]
        public void Setup()
        {
            resolver = new Resolver();

            var contextStorage = new ContextStorage();
            contextScopeResolver = new ContextScopeResolver(new ContextSupportResolver(contextStorage), contextStorage);

            childScopeResolver = new ChildScopeResolver(new ChildSupportResolver());
        }

        [Benchmark]
        public object Resolver()
        {
            return resolver.GetInstance(RequestType);
        }

        [Benchmark]
        public object ContextScopeResolver()
        {
            return contextScopeResolver.GetInstance(RequestType);
        }

        [Benchmark]
        public object ChildScopeResolver()
        {
            return childScopeResolver.GetInstance(RequestType);
        }
    }

    // Interface

    public interface ILifetimeSupport
    {
        object GetOrCreate(Type type, Func<object> func);
    }

    public interface IChildResolver
    {
        object GetInstance(ILifetimeSupport container, Type type);
    }

    public interface IResolver : ILifetimeSupport, IDisposable
    {
        object GetInstance(Type type);
    }

    // Child scope

    public sealed class ChildSupportResolver : IChildResolver
    {
        private readonly Dictionary<Type, object> dummy = new Dictionary<Type, object>();

        private object Factory()
        {
            return new Data();
        }

        public object GetInstance(ILifetimeSupport container, Type type)
        {
            dummy.TryGetValue(type, out _);
            return container.GetOrCreate(type, Factory);
        }
    }

    public sealed class ChildScopeResolver : IResolver
    {
        private readonly ChildSupportResolver provider;

        private readonly Dictionary<Type, object> cache = new Dictionary<Type, object>();

        public ChildScopeResolver(ChildSupportResolver provider)
        {
            this.provider = provider;
        }

        public void Dispose()
        {
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

    // Context scope

    public sealed class ContextStorage
    {
        private readonly AsyncLocal<Dictionary<Type, object>> store = new AsyncLocal<Dictionary<Type, object>>();

        public Dictionary<Type, object> Store
        {
            get => store.Value;
            set => store.Value = value;
        }
    }

    public sealed class ContextSupportResolver : IResolver
    {
        private readonly ContextStorage storage;

        public ContextSupportResolver(ContextStorage storage)
        {
            this.storage = storage;
        }

        public void Dispose()
        {
        }

        public object GetInstance(Type type)
        {
            var cache = storage.Store;
            if (cache is null)
            {
                return new Data();
            }

            if (!cache.TryGetValue(type, out var value))
            {
                value = new Data();
                cache[type] = value;
            }

            return value;
        }

        public object GetOrCreate(Type type, Func<object> func)
        {
            throw new NotSupportedException();
        }

    }


    public sealed class ContextScopeResolver : IResolver
    {
        private readonly ContextSupportResolver resolver;

        private readonly ContextStorage storage;

        public ContextScopeResolver(ContextSupportResolver resolver, ContextStorage storage)
        {
            this.resolver = resolver;
            this.storage = storage;
            storage.Store = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            storage.Store.Clear();
        }

        public object GetInstance(Type type)
        {
            return resolver.GetInstance(type);
        }

        public object GetOrCreate(Type type, Func<object> func)
        {
            throw new NotSupportedException();
        }
    }

    // Core

    public sealed class Resolver : IResolver
    {
        private readonly Dictionary<Type, object> dummy = new Dictionary<Type, object>();

        public void Dispose()
        {
        }

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

    // Data

    public class Data
    {
    }
}
