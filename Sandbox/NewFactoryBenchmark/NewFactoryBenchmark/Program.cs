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
            Add(Job.LongRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private const int N = 1_000;

        private Func<Data0> funcFunc0;
        private Func<Data1> funcFunc1;
        private Func<Data2> funcFunc2;

        private Func<Data0> classFunc0;
        private Func<Data1> classFunc1;
        private Func<Data2> classFunc2;

        private Func<Data0> staticFunc0;
        private Func<Data1> staticFunc1;
        private Func<Data2> staticFunc2;

        [GlobalSetup]
        public void Setup()
        {
            funcFunc0 = CreateFunc0();
            funcFunc1 = CreateFunc1(funcFunc0);
            funcFunc2 = CreateFunc2(funcFunc0, funcFunc1);

            var classResolver0 = new ClassResolver0();
            classFunc0 = classResolver0.Resolve;
            var classResolver1 = new ClassResolver1 { arg1Resolver = classFunc0 };
            classFunc1 = classResolver1.Resolve;
            var classResolver2 = new ClassResolver2 { arg1Resolver = classFunc0, arg2Resolver = classFunc1 };
            classFunc2 = classResolver2.Resolve;

            staticFunc0 = StaticResolver0.Resolve;
            StaticResolver1.arg1Resolver = staticFunc0;
            staticFunc1 = StaticResolver1.Resolve;
            StaticResolver2.arg1Resolver = staticFunc0;
            StaticResolver2.arg2Resolver = staticFunc1;
            staticFunc2 = StaticResolver2.Resolve;
        }

        private static Func<Data0> CreateFunc0() => () => new Data0();

        private static Func<Data1> CreateFunc1(Func<Data0> f0) => () => new Data1(f0());

        private static Func<Data2> CreateFunc2(Func<Data0> f0, Func<Data1> f1) => () => new Data2(f0(), f1());

        [Benchmark(OperationsPerInvoke = N)]
        public void FuncFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                funcFunc2();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void ClassFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                classFunc2();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void StaticFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                staticFunc2();
            }
        }
    }

    // Resolver

    public sealed class ClassResolver0
    {
        public Data0 Resolve() => new Data0();
    }

    public sealed class ClassResolver1
    {
        public Func<Data0> arg1Resolver;

        public Data1 Resolve() => new Data1(arg1Resolver());
    }

    public sealed class ClassResolver2
    {
        public Func<Data0> arg1Resolver;

        public Func<Data1> arg2Resolver;

        public Data2 Resolve() => new Data2(arg1Resolver(), arg2Resolver());
    }

    // Resolver(static)

    public static class StaticResolver0
    {
        public static Data0 Resolve() => new Data0();
    }

    public static class StaticResolver1
    {
        public static Func<Data0> arg1Resolver;

        public static Data1 Resolve() => new Data1(arg1Resolver());
    }

    public static class StaticResolver2
    {
        public static Func<Data0> arg1Resolver;

        public static Func<Data1> arg2Resolver;

        public static Data2 Resolve() => new Data2(arg1Resolver(), arg2Resolver());
    }

    // Data

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
