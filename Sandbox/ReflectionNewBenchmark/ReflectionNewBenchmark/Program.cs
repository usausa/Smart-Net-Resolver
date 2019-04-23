namespace ReflectionNewBenchmark
{
    using System;
    using System.Reflection;

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
            BenchmarkRunner.Run<BenchmarkConfig.Benchmark>();
        }
    }

    public sealed class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.Default, MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(Job.ShortRun);
        }

        [Config(typeof(BenchmarkConfig))]
        public class Benchmark
        {
            private readonly Type Type0 = typeof(Data0);
            private readonly Type Type1 = typeof(Data1);
            private readonly Type Type2 = typeof(Data2);

            private readonly ConstructorInfo Ctor0 = typeof(Data0).GetConstructors()[0];
            private readonly ConstructorInfo Ctor1 = typeof(Data1).GetConstructors()[0];
            private readonly ConstructorInfo Ctor2 = typeof(Data2).GetConstructors()[0];

            [Benchmark]
            public object TypedActivator0() => Activator.CreateInstance<Data0>();

            [Benchmark]
            public object Activator0() => Activator.CreateInstance(Type0);

            [Benchmark]
            public object Activator1() => Activator.CreateInstance(Type1, 0);

            [Benchmark]
            public object Activator2() => Activator.CreateInstance(Type2, 0, string.Empty);

            [Benchmark]
            public object Constructor0() => Ctor0.Invoke(null);


            [Benchmark]
            public object Constructor1() => Ctor1.Invoke(new object[] { 0 });

            [Benchmark]
            public object Constructor2() => Ctor2.Invoke(new object[] { 0, string.Empty });
        }
    }

    public class Data0
    {
    }

    public class Data1
    {
        public Data1(int arg0)
        {
        }
    }

    public class Data2
    {
        public Data2(int arg0, string arg2)
        {
        }
    }
}
