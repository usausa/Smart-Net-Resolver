namespace NewDynamicFactoryBenchmark
{
    using System;
    using System.Reflection;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;

    using Smart.Resolver.Builders;

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

        private static readonly Func<IResolver, object>[] EmptyFactories = Array.Empty<Func<IResolver, object>>();
        private static readonly Action<IResolver, object>[] EmptyActions = Array.Empty<Action<IResolver, object>>();

        private static readonly ConstructorInfo ci0 = typeof(Data0).GetConstructors()[0];
        private static readonly ConstructorInfo ci1 = typeof(Data1).GetConstructors()[0];
        private static readonly ConstructorInfo ci2 = typeof(Data2).GetConstructors()[0];
        private static readonly ConstructorInfo ci4 = typeof(Data4).GetConstructors()[0];

        private readonly Func<IResolver, object> stringFactory = r => string.Empty;
        private readonly Func<IResolver, object> intFactory = r => 0;

        private Func<IResolver, object> oldDynamicFactory0;
        private Func<IResolver, object> oldDynamicFactory1;
        private Func<IResolver, object> oldDynamicFactory2;
        private Func<IResolver, object> oldDynamicFactory4;

        private Func<IResolver, object> newDynamicFactory0;
        private Func<IResolver, object> newDynamicFactory1;
        private Func<IResolver, object> newDynamicFactory2;
        private Func<IResolver, object> newDynamicFactory4;

        [GlobalSetup]
        public void Setup()
        {
            var oldBuilder = new OldEmitFactoryBuilder();
            var newBuilder = new EmitFactoryBuilder();

            oldDynamicFactory0 = oldBuilder.CreateFactory(ci0, EmptyFactories, EmptyActions);
            oldDynamicFactory1 = oldBuilder.CreateFactory(ci1, new[] { stringFactory }, EmptyActions);
            oldDynamicFactory2 = oldBuilder.CreateFactory(ci2, new[] { stringFactory, intFactory }, EmptyActions);
            oldDynamicFactory4 = oldBuilder.CreateFactory(ci4, new[] { stringFactory, intFactory, stringFactory, intFactory }, EmptyActions);

            newDynamicFactory0 = newBuilder.CreateFactory(ci0, EmptyFactories, EmptyActions);
            newDynamicFactory1 = newBuilder.CreateFactory(ci1, new[] { stringFactory }, EmptyActions);
            newDynamicFactory2 = newBuilder.CreateFactory(ci2, new[] { stringFactory, intFactory }, EmptyActions);
            newDynamicFactory4 = newBuilder.CreateFactory(ci4, new[] { stringFactory, intFactory, stringFactory, intFactory }, EmptyActions);
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void OldDynamicFactory0()
        {
            for (var i = 0; i < N; i++)
            {
                oldDynamicFactory0(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NewDynamicFactory0()
        {
            for (var i = 0; i < N; i++)
            {
                newDynamicFactory0(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void OldDynamicFactory1()
        {
            for (var i = 0; i < N; i++)
            {
                oldDynamicFactory1(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NewDynamicFactory1()
        {
            for (var i = 0; i < N; i++)
            {
                newDynamicFactory1(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void OldDynamicFactory2()
        {
            for (var i = 0; i < N; i++)
            {
                oldDynamicFactory2(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NewDynamicFactory2()
        {
            for (var i = 0; i < N; i++)
            {
                newDynamicFactory2(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void OldDynamicFactory4()
        {
            for (var i = 0; i < N; i++)
            {
                oldDynamicFactory4(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void NewDynamicFactory4()
        {
            for (var i = 0; i < N; i++)
            {
                newDynamicFactory4(null);
            }
        }
    }

    // Data

    public sealed class Data0
    {
    }

    public sealed class Data1
    {
        public string Parameter1 { get; }

        public Data1(string parameter1)
        {
            Parameter1 = parameter1;
        }
    }

    public sealed class Data2
    {
        public string Parameter1 { get; }

        public int Parameter2 { get; }

        public Data2(string parameter1, int parameter2)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
        }
    }

    public sealed class Data4
    {
        public string Parameter1 { get; }

        public int Parameter2 { get; }

        public string Parameter3 { get; }

        public int Parameter4 { get; }

        public Data4(string parameter1, int parameter2, string parameter3, int parameter4)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            Parameter3 = parameter3;
            Parameter4 = parameter4;
        }
    }
}
