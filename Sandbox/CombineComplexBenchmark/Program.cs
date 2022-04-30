namespace CombineComplexBenchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Smart.Collections.Concurrent;

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

[Config(typeof(BenchmarkConfig))]
public class Benchmark
{
    private const int Loop = 3000;

    private ThreadsafeTypeHashArrayMap<Func<object>> map;

    [GlobalSetup]
    public void Setup()
    {
        var singleton = (object)new Singleton();
        var singletonFactory = (Func<object>)(() => singleton);

        var transientFactory = (Func<object>)(() => new Transient());

        var combineFactory = (Func<object>)(() => new Combine(
            (Singleton)singletonFactory(),
            (Transient)transientFactory()));

        var componentFactory = (Func<object>)(() => new Component((Singleton)singletonFactory()));
        var complexFactory = (Func<object>)(() => new Complex(
            (Singleton)singletonFactory(),
            (Singleton)singletonFactory(),
            (Component)componentFactory(),
            (Component)componentFactory(),
            (Transient)transientFactory(),
            (Transient)transientFactory()));

        var combine2Factory = (Func<object>)(() => new Combine2(
            (Singleton)singleton,
            (Transient)transientFactory()));

        var component2Factory = (Func<object>)(() => new Component((Singleton)singleton));
        var complex2Factory = (Func<object>)(() => new Complex2(
            (Singleton)singleton,
            (Singleton)singleton,
            (Component)component2Factory(),
            (Component)component2Factory(),
            (Transient)transientFactory(),
            (Transient)transientFactory()));

        singletonFactory();
        transientFactory();
        combineFactory();
        complexFactory();
        combine2Factory();
        complex2Factory();

        map = new ThreadsafeTypeHashArrayMap<Func<object>>();
        map.AddIfNotExist(typeof(Singleton), singletonFactory);
        map.AddIfNotExist(typeof(Transient), transientFactory);
        map.AddIfNotExist(typeof(Combine), combineFactory);
        map.AddIfNotExist(typeof(Complex), complexFactory);
        map.AddIfNotExist(typeof(Combine2), combine2Factory);
        map.AddIfNotExist(typeof(Complex2), complex2Factory);
    }

    private object Create(Type type)
    {
        map.TryGetValue(type, out var func);
        return func();
    }

    [Benchmark]
    public void CreateSingleton()
    {
        for (var i = 0; i < Loop; i++)
        {
            Create(typeof(Singleton));
        }
    }

    [Benchmark]
    public void CreateTransient()
    {
        for (var i = 0; i < Loop; i++)
        {
            Create(typeof(Transient));
        }
    }

    [Benchmark]
    public void CreateCombine()
    {
        for (var i = 0; i < Loop; i++)
        {
            Create(typeof(Combine));
        }
    }

    [Benchmark]
    public void CreateComplex()
    {
        for (var i = 0; i < Loop; i++)
        {
            Create(typeof(Complex));
        }
    }

    [Benchmark]
    public void CreateCombine2()
    {
        for (var i = 0; i < Loop; i++)
        {
            Create(typeof(Combine2));
        }
    }

    [Benchmark]
    public void CreateComplex2()
    {
        for (var i = 0; i < Loop; i++)
        {
            Create(typeof(Complex2));
        }
    }
}

#pragma warning disable IDE0060
// ReSharper disable UnusedParameter.Local
public class Singleton
{
}

public class Transient
{
}

public class Combine
{
    public Combine(Singleton singleton, Transient transient)
    {
    }
}

public class Combine2
{
    public Combine2(Singleton singleton, Transient transient)
    {
    }
}

public class Component
{
    public Component(Singleton singleton)
    {
    }
}

public class Complex
{
    public Complex(Singleton singleton1, Singleton singleton2, Component component1, Component component2, Transient transient1, Transient transient2)
    {
    }
}

public class Complex2
{
    public Complex2(Singleton singleton1, Singleton singleton2, Component component1, Component component2, Transient transient1, Transient transient2)
    {
    }
}
