namespace FuncBenchmark
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

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private const int N = 1_000_000;

        private Func<object> nonSealed0Func;
        private Func<object> nonSealed1Func;
        private Func<object> nonSealed2Func;

        private Func<object> sealed0Func;
        private Func<object> sealed1Func;
        private Func<object> sealed2Func;

        private Func<object> nonInterface0Func;
        private Func<object> nonInterface1Func;
        private Func<object> nonInterface2Func;

        private Func<object> direct0Func;
        private Func<object> direct1Func;
        private Func<object> direct2Func;

        private Func<object, object> direct0Func1;
        private Func<object, object> direct1Func1;
        private Func<object, object> direct2Func1;

        [GlobalSetup]
        public void Setup()
        {
            var nonSealedResolver = new NonSealedObjectResolver();
            var nonSelaedResolver0 = new NonSealedResolver0();
            var nonSelaedResolver1 = new NonSealedResolver1(nonSealedResolver.Resolve);
            var nonSelaedResolver2 = new NonSealedResolver2(nonSealedResolver.Resolve, nonSealedResolver.Resolve);
            nonSealed0Func = nonSelaedResolver0.Resolve;
            nonSealed1Func = nonSelaedResolver1.Resolve;
            nonSealed2Func = nonSelaedResolver2.Resolve;

            var sealedResolver = new SealedObjectResolver();
            var sealedResolver0 = new SealedResolver0();
            var sealedResolver1 = new SealedResolver1(sealedResolver.Resolve);
            var sealedResolver2 = new SealedResolver2(sealedResolver.Resolve, sealedResolver.Resolve);
            sealed0Func = sealedResolver0.Resolve;
            sealed1Func = sealedResolver1.Resolve;
            sealed2Func = sealedResolver2.Resolve;

            var nonInterfaceResolver = new NonInterfaceObjectResolver();
            var nonInterfaceResolver0 = new NonInterfaceResolver0();
            var nonInterfaceResolver1 = new NonInterfaceResolver1(nonInterfaceResolver.Resolve);
            var nonInterfaceResolver2 = new NonInterfaceResolver2(nonInterfaceResolver.Resolve, nonInterfaceResolver.Resolve);
            nonInterface0Func = nonInterfaceResolver0.Resolve;
            nonInterface1Func = nonInterfaceResolver1.Resolve;
            nonInterface2Func = nonInterfaceResolver2.Resolve;

            var directResolver = (Func<object>)(() => new object());
            direct0Func = () => new Data0();
            direct1Func = () => new Data1(directResolver);
            direct2Func = () => new Data2(directResolver, directResolver);
            direct0Func1 = x => new Data0();
            direct1Func1 = x => new Data1(directResolver);
            direct2Func1 = x => new Data2(directResolver, directResolver);
        }

        // NonSealed

        [Benchmark(OperationsPerInvoke = N)]
        public void NonSealed0()
        {
            for (var i = 0; i < N; i++)
            {
                nonSealed0Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NonSealed1()
        {
            for (var i = 0; i < N; i++)
            {
                nonSealed1Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NonSealed2()
        {
            for (var i = 0; i < N; i++)
            {
                nonSealed2Func();
            }
        }

        // Sealed

        [Benchmark(OperationsPerInvoke = N)]
        public void Sealed0()
        {
            for (var i = 0; i < N; i++)
            {
                sealed0Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Sealed1()
        {
            for (var i = 0; i < N; i++)
            {
                sealed1Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Sealed2()
        {
            for (var i = 0; i < N; i++)
            {
                sealed2Func();
            }
        }

        // NonInterface

        [Benchmark(OperationsPerInvoke = N)]
        public void NonInterface0()
        {
            for (var i = 0; i < N; i++)
            {
                nonInterface0Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NonInterface1()
        {
            for (var i = 0; i < N; i++)
            {
                nonInterface1Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NonInterface2()
        {
            for (var i = 0; i < N; i++)
            {
                nonInterface2Func();
            }
        }

        // Direct

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct0()
        {
            for (var i = 0; i < N; i++)
            {
                direct0Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct1()
        {
            for (var i = 0; i < N; i++)
            {
                direct1Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct2()
        {
            for (var i = 0; i < N; i++)
            {
                direct2Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct01()
        {
            for (var i = 0; i < N; i++)
            {
                direct0Func1(this);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct11()
        {
            for (var i = 0; i < N; i++)
            {
                direct1Func1(this);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct21()
        {
            for (var i = 0; i < N; i++)
            {
                direct2Func1(this);
            }
        }
    }

    public class Data0
    {
    }

    public class Data1
    {
        public object Arg1 { get; }

        public Data1(object arg1)
        {
            Arg1 = arg1;
        }
    }

    public class Data2
    {
        public object Arg1 { get; }

        public object Arg2 { get; }

        public Data2(object arg1, object arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
    }

    public interface IResolver
    {
        object Resolve();
    }

    // NonSealed

    public class NonSealedObjectResolver : IResolver
    {
        public object Resolve()
        {
            return new object();
        }
    }

    public class NonSealedResolver0 : IResolver
    {
        public object Resolve()
        {
            return new Data0();
        }
    }

    public class NonSealedResolver1 : IResolver
    {
        private readonly Func<object> arg1Resolver;

        public NonSealedResolver1(Func<object> arg1Resolver)
        {
            this.arg1Resolver = arg1Resolver;
        }

        public object Resolve()
        {
            return new Data1(arg1Resolver());
        }
    }

    public class NonSealedResolver2 : IResolver
    {
        private readonly Func<object> arg1Resolver;

        private readonly Func<object> arg2Resolver;

        public NonSealedResolver2(Func<object> arg1Resolver, Func<object> arg2Resolver)
        {
            this.arg1Resolver = arg1Resolver;
            this.arg2Resolver = arg2Resolver;
        }

        public object Resolve()
        {
            return new Data2(arg1Resolver(), arg2Resolver());
        }
    }

    // Sealed

    public sealed class SealedObjectResolver : IResolver
    {
        public object Resolve()
        {
            return new object();
        }
    }

    public sealed class SealedResolver0 : IResolver
    {
        public object Resolve()
        {
            return new Data0();
        }
    }

    public sealed class SealedResolver1 : IResolver
    {
        private readonly Func<object> arg1Resolver;

        public SealedResolver1(Func<object> arg1Resolver)
        {
            this.arg1Resolver = arg1Resolver;
        }

        public object Resolve()
        {
            return new Data1(arg1Resolver());
        }
    }

    public sealed class SealedResolver2 : IResolver
    {
        private readonly Func<object> arg1Resolver;

        private readonly Func<object> arg2Resolver;

        public SealedResolver2(Func<object> arg1Resolver, Func<object> arg2Resolver)
        {
            this.arg1Resolver = arg1Resolver;
            this.arg2Resolver = arg2Resolver;
        }

        public object Resolve()
        {
            return new Data2(arg1Resolver(), arg2Resolver());
        }
    }

    // NonInterface

    public class NonInterfaceObjectResolver
    {
        public object Resolve()
        {
            return new object();
        }
    }

    public class NonInterfaceResolver0
    {
        public object Resolve()
        {
            return new Data0();
        }
    }

    public class NonInterfaceResolver1
    {
        private readonly Func<object> arg1Resolver;

        public NonInterfaceResolver1(Func<object> arg1Resolver)
        {
            this.arg1Resolver = arg1Resolver;
        }

        public object Resolve()
        {
            return new Data1(arg1Resolver());
        }
    }

    public class NonInterfaceResolver2
    {
        private readonly Func<object> arg1Resolver;

        private readonly Func<object> arg2Resolver;

        public NonInterfaceResolver2(Func<object> arg1Resolver, Func<object> arg2Resolver)
        {
            this.arg1Resolver = arg1Resolver;
            this.arg2Resolver = arg2Resolver;
        }

        public object Resolve()
        {
            return new Data2(arg1Resolver(), arg2Resolver());
        }
    }
}
