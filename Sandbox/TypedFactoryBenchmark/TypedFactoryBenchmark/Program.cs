namespace TypedFactoryBenchmark;

using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Smart.Collections.Concurrent;

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
    private readonly FuncResolver transientFuncResolver = new FuncResolver();

    private readonly TypedFuncResolver transientTypedFucResolver = new TypedFuncResolver();

    [GlobalSetup]
    public void Setup()
    {
        var simple1Func = (Func<object>)(() => new Simple1());
        var simple2Func = (Func<object>)(() => new Simple2());
        var simple3Func = (Func<object>)(() => new Simple3());
        var combine1Func = (Func<object>)(() => new Combine1((Simple1)simple1Func()));
        var combine2Func = (Func<object>)(() => new Combine2((Simple2)simple2Func()));
        var combine3Func = (Func<object>)(() => new Combine3((Simple3)simple3Func()));
        var complexFunc = (Func<object>)(() => new Complex(
            (Simple1)simple1Func(),
            (Simple2)simple2Func(),
            (Simple3)simple3Func(),
            (Combine1)combine1Func(),
            (Combine2)combine2Func(),
            (Combine3)combine3Func()));
        transientFuncResolver.Add(typeof(Complex), complexFunc);

        var simple1TypedFunc = (Func<Simple1>)(() => new Simple1());
        var simple2TypedFunc = (Func<Simple2>)(() => new Simple2());
        var simple3TypedFunc = (Func<Simple3>)(() => new Simple3());
        var combine1TypedFunc = (Func<Combine1>)(() => new Combine1(simple1TypedFunc()));
        var combine2TypedFunc = (Func<Combine2>)(() => new Combine2(simple2TypedFunc()));
        var combine3TypedFunc = (Func<Combine3>)(() => new Combine3(simple3TypedFunc()));
        var complexTypedFunc = (Func<Complex>)(() => new Complex(
            simple1TypedFunc(),
            simple2TypedFunc(),
            simple3TypedFunc(),
            combine1TypedFunc(),
            combine2TypedFunc(),
            combine3TypedFunc()));
        transientTypedFucResolver.Add(complexTypedFunc);
    }

    [Benchmark]
    public object TransientComplexFunc()
    {
        return transientFuncResolver.Get<Complex>();
    }

    [Benchmark]
    public object TransientComplexTypedFunc()
    {
        return transientTypedFucResolver.Get<Complex>();
    }
}

public class Simple1
{
}

public class Simple2
{
}

public class Simple3
{
}

public class Combine1
{
    public Simple1 Simple1 { get; }

    public Combine1(Simple1 simple1)
    {
        Simple1 = simple1;
    }
}

public class Combine2
{
    public Simple2 Simple2 { get; }

    public Combine2(Simple2 simple2)
    {
        Simple2 = simple2;
    }
}

public class Combine3
{
    public Simple3 Simple3 { get; }

    public Combine3(Simple3 simple3)
    {
        Simple3 = simple3;
    }
}

public class Complex
{
    public Simple1 Simple1 { get; }

    public Simple2 Simple2 { get; }

    public Simple3 Simple3 { get; }

    public Combine1 Combine1 { get; }

    public Combine2 Combine2 { get; }

    public Combine3 Combine3 { get; }

    public Complex(Simple1 simple1, Simple2 simple2, Simple3 simple3, Combine1 combine1, Combine2 combine2, Combine3 combine3)
    {
        Simple1 = simple1;
        Simple2 = simple2;
        Simple3 = simple3;
        Combine1 = combine1;
        Combine2 = combine2;
        Combine3 = combine3;
    }
}

public class FuncResolver
{
    private readonly ThreadsafeTypeHashArrayMap<Func<object>> factories = new ThreadsafeTypeHashArrayMap<Func<object>>();

    public void Add(Type type, Func<object> factory)
    {
        factories.AddIfNotExist(type, factory);
    }

    public object Get(Type type)
    {
        return factories.TryGetValue(type, out var factory) ? factory() : null;
    }

    public T Get<T>()
    {
        return factories.TryGetValue(typeof(T), out var factory) ? (T)(factory()) : default;
    }
}

public class TypedFuncResolver
{
    private readonly ThreadsafeTypeHashArrayMap<Func<object>> factories = new ThreadsafeTypeHashArrayMap<Func<object>>();

    public void Add<T>(Func<T> factory) where T : class
    {
        factories.AddIfNotExist(typeof(T), factory);
    }

    public object Get(Type type)
    {
        return factories.TryGetValue(type, out var factory) ? factory() : null;
    }

    public T Get<T>() where T : class
    {
        return factories.TryGetValue(typeof(T), out var factory) ? ((Func<T>)factory)() : default;
    }
}
