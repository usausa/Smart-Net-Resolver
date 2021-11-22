namespace ReflectionNewBenchmark;

using System;
using System.Reflection;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
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

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddExporter(MarkdownExporter.Default, MarkdownExporter.GitHub);
        AddColumn(
            StatisticColumn.Mean,
            StatisticColumn.Min,
            StatisticColumn.Max,
            StatisticColumn.P90,
            StatisticColumn.Error,
            StatisticColumn.StdDev);
        AddDiagnoser(MemoryDiagnoser.Default);
        AddJob(Job.ShortRun);
    }
}

#pragma warning disable CA1822
[Config(typeof(BenchmarkConfig))]
public class Benchmark
{
    private readonly Func<Data0> func0A = Builder.CreateFactoryA<Data0>();
    private readonly Func<Data0> func0B = Builder.CreateFactoryB<Data0>();

    private readonly Type type0 = typeof(Data0);
    private readonly Type type1 = typeof(Data1);
    private readonly Type type2 = typeof(Data2);

    private readonly ConstructorInfo ctor0 = typeof(Data0).GetConstructors()[0];
    private readonly ConstructorInfo ctor1 = typeof(Data1).GetConstructors()[0];
    private readonly ConstructorInfo ctor2 = typeof(Data2).GetConstructors()[0];

    [Benchmark]
    public Data0 FuncActivator0A() => func0A();

    [Benchmark]
    public Data0 FuncActivator0B() => func0B();

    [Benchmark]
    public Data0 TypedActivator0() => Activator.CreateInstance<Data0>();

    [Benchmark]
    public Data0 Activator0() => (Data0)Activator.CreateInstance(type0);

    [Benchmark]
    public Data1 Activator1() => (Data1)Activator.CreateInstance(type1, 0);

    [Benchmark]
    public Data2 Activator2() => (Data2)Activator.CreateInstance(type2, 0, string.Empty);

    [Benchmark]
    public Data0 Constructor0() => (Data0)ctor0.Invoke(null);

    [Benchmark]
    public Data1 Constructor1() => (Data1)ctor1.Invoke(new object[] { 0 });

    [Benchmark]
    public Data2 Constructor2() => (Data2)ctor2.Invoke(new object[] { 0, string.Empty });
}

// ReSharper disable UnusedParameter.Local
#pragma warning disable IDE0060

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
