namespace NewFactoryBenchmark
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
        public static void Main()
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }

    public sealed class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.Default, MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(Job.MediumRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private const int N = 1_000;

        private Func<Data0> direct0Func;
        private Func<Data1> direct1Func;
        private Func<Data2> direct2Func;

        private Func<Data0> func0Func;
        private Func<Data1> func1Func;
        private Func<Data2> func2Func;

        [GlobalSetup]
        public void Setup()
        {
            var direct0Resolver = new DirectResolver0();
            direct0Func = direct0Resolver.Resolve;
            var direct1Resolver = new DirectResolver1 { arg1Resolver = direct0Func };
            direct1Func = direct1Resolver.Resolve;
            var direct2Resolver = new DirectResolver2 { arg1Resolver = direct0Func, arg2Resolver = direct1Func };
            direct2Func = direct2Resolver.Resolve;

            func0Func = CreateFunc0();
            func1Func = CreateFunc1(func0Func);
            func2Func = CreateFunc2(func0Func, func1Func);
        }

        private static Func<Data0> CreateFunc0() => () => new Data0();

        private static Func<Data1> CreateFunc1(Func<Data0> f0) => () => new Data1(f0());

        private static Func<Data2> CreateFunc2(Func<Data0> f0, Func<Data1> f1) => () => new Data2(f0(), f1());

        [Benchmark(OperationsPerInvoke = N)]
        public void Direct2Func()
        {
            for (var i = 0; i < N; i++)
            {
                direct2Func();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void Func2Func()
        {
            for (var i = 0; i < N; i++)
            {
                func2Func();
            }
        }
    }

    public sealed class DirectResolver0
    {
        public Data0 Resolve() => new Data0();
    }

    public sealed class DirectResolver1
    {
        public Func<Data0> arg1Resolver;

        public Data1 Resolve() => new Data1(arg1Resolver());
    }

    public sealed class DirectResolver2
    {
        public Func<Data0> arg1Resolver;

        public Func<Data1> arg2Resolver;

        public Data2 Resolve() => new Data2(arg1Resolver(), arg2Resolver());
    }

    public sealed class Data0
    {
    }

    public sealed class Data1
    {
        public Data0 Arg1 { get; }

        public Data1(Data0 arg1)
        {
            Arg1 = arg1;
        }
    }

    public sealed class Data2
    {
        public Data0 Arg1 { get; }

        public Data1 Arg2 { get; }

        public Data2(Data0 arg1, Data1 arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
    }
}
