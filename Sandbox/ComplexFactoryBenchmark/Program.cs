namespace ComplexFactoryBenchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using ComplexFactoryBenchmark.Classes;

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
    private Func<object> complexFactory;

    private Func<Complex> typedComplexFactory;

    [GlobalSetup]
    public void Setup()
    {
        var factory = new Factory();

        complexFactory = factory.Complex(
            factory.Singleton1(),
            factory.Singleton2(),
            factory.Singleton3(),
            factory.Combined1(factory.Singleton1()),
            factory.Combined2(factory.Singleton2()),
            factory.Combined3(factory.Singleton3()));
        typedComplexFactory = factory.TypedComplex(
            factory.TypedSingleton1(),
            factory.TypedSingleton2(),
            factory.TypedSingleton3(),
            factory.TypedCombined1(factory.TypedSingleton1()),
            factory.TypedCombined2(factory.TypedSingleton2()),
            factory.TypedCombined3(factory.TypedSingleton3()));
    }

    [Benchmark]
    public object Complex()
    {
        return complexFactory();
    }

    [Benchmark]
    public object TypedComplex()
    {
        return typedComplexFactory();
    }
}

#pragma warning disable CA1822
public class Factory
{
    // Singleton

    public Func<object> Singleton1()
    {
        var obj = new Singleton1();
        return () => obj;
    }

    public Func<object> Singleton2()
    {
        var obj = new Singleton2();
        return () => obj;
    }

    public Func<object> Singleton3()
    {
        var obj = new Singleton3();
        return () => obj;
    }

    public Func<ISingleton1> TypedSingleton1()
    {
        var obj = new Singleton1();
        return () => obj;
    }

    public Func<ISingleton2> TypedSingleton2()
    {
        var obj = new Singleton2();
        return () => obj;
    }

    public Func<ISingleton3> TypedSingleton3()
    {
        var obj = new Singleton3();
        return () => obj;
    }

    // Combined

    public Func<object> Combined1(Func<object> singletonFunc1)
    {
        return () => new Combined1((ISingleton1)singletonFunc1());
    }

    public Func<object> Combined2(Func<object> singletonFunc2)
    {
        return () => new Combined2((ISingleton2)singletonFunc2());
    }

    public Func<object> Combined3(Func<object> singletonFunc3)
    {
        return () => new Combined3((ISingleton3)singletonFunc3());
    }

    public Func<Combined1> TypedCombined1(Func<ISingleton1> singletonFunc1)
    {
        return () => new Combined1(singletonFunc1());
    }

    public Func<Combined2> TypedCombined2(Func<ISingleton2> singletonFunc2)
    {
        return () => new Combined2(singletonFunc2());
    }

    public Func<Combined3> TypedCombined3(Func<ISingleton3> singletonFunc3)
    {
        return () => new Combined3(singletonFunc3());
    }

    // Complex

    public Func<object> Complex(
        Func<object> singletonFactory1,
        Func<object> singletonFactory2,
        Func<object> singletonFactory3,
        Func<object> combinedFactory1,
        Func<object> combinedFactory2,
        Func<object> combinedFactory3)
    {
        return () => new Complex(
            (ISingleton1)singletonFactory1(),
            (ISingleton2)singletonFactory2(),
            (ISingleton3)singletonFactory3(),
            (Combined1)combinedFactory1(),
            (Combined2)combinedFactory2(),
            (Combined3)combinedFactory3());
    }

    public Func<Complex> TypedComplex(
        Func<ISingleton1> singletonFactory1,
        Func<ISingleton2> singletonFactory2,
        Func<ISingleton3> singletonFactory3,
        Func<Combined1> combinedFactory1,
        Func<Combined2> combinedFactory2,
        Func<Combined3> combinedFactory3)
    {
        return () => new Complex(
            singletonFactory1(),
            singletonFactory2(),
            singletonFactory3(),
            combinedFactory1(),
            combinedFactory2(),
            combinedFactory3());
    }
}
