namespace ReflectionNewBenchmark;

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
        Add(Job.MediumRun);
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly Func<Data0> Func0a = Builder.CreateFactoryA<Data0>();
        private readonly Func<Data0> Func0b = Builder.CreateFactoryB<Data0>();

        private readonly Type Type0 = typeof(Data0);
        private readonly Type Type1 = typeof(Data1);
        private readonly Type Type2 = typeof(Data2);

        private readonly ConstructorInfo Ctor0 = typeof(Data0).GetConstructors()[0];
        private readonly ConstructorInfo Ctor1 = typeof(Data1).GetConstructors()[0];
        private readonly ConstructorInfo Ctor2 = typeof(Data2).GetConstructors()[0];

        [Benchmark]
        public Data0 FuncActivator0a() => Func0a();

        [Benchmark]
        public Data0 FuncActivator0b() => Func0b();

        [Benchmark]
        public Data0 TypedActivator0() => Activator.CreateInstance<Data0>();

        [Benchmark]
        public Data0 Activator0() => (Data0)Activator.CreateInstance(Type0);

        [Benchmark]
        public Data1 Activator1() => (Data1)Activator.CreateInstance(Type1, 0);

        [Benchmark]
        public Data2 Activator2() => (Data2)Activator.CreateInstance(Type2, 0, string.Empty);

        [Benchmark]
        public Data0 Constructor0() => (Data0)Ctor0.Invoke(null);


        [Benchmark]
        public Data1 Constructor1() => (Data1)Ctor1.Invoke(new object[] { 0 });

        [Benchmark]
        public Data2 Constructor2() => (Data2)Ctor2.Invoke(new object[] { 0, string.Empty });
    }
}

public static class Builder
{
    public static Func<T> CreateFactoryA<T>()
    {
        var type = typeof(T);
        return () => (T)Activator.CreateInstance(type);
    }

    public static Func<T> CreateFactoryB<T>()
    {
        return Activator.CreateInstance<T>;
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
