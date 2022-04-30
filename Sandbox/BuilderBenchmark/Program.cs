namespace BuilderBenchmark;

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
        AddJob(Job.LongRun);
    }
}

public class Benchmark
{
    private const int N = 1000;

    private IFactory factory0;
    private IFactory factory1;
    private IFactory factory2;
    private IFactory factory4;
    private IFactory factoryX;

    private Func<IResolver, object> func0;
    private Func<IResolver, object> func1;
    private Func<IResolver, object> func2;
    private Func<IResolver, object> func4;
    private Func<IResolver, object> funcX;

    [GlobalSetup]
    public void Setup()
    {
        var dataFactory = new Data0Factory();
        factory0 = dataFactory;
        factory1 = new Data1InterfaceFactory(dataFactory);
        factory2 = new Data2InterfaceFactory(dataFactory, dataFactory);
        factory4 = new Data4InterfaceFactory(dataFactory, dataFactory, dataFactory, dataFactory);
        factoryX = new ComplexDataInterfaceFactory(factory0, factory1, factory2, factory4);

        func0 = dataFactory.Create;
        var func1Factory = new Data1FuncFactory(func0);
        func1 = func1Factory.Create;
        var func2Factory = new Data2FuncFactory(func0, func0);
        func2 = func2Factory.Create;
        var func4Factory = new Data4FuncFactory(func0, func0, func0, func0);
        func4 = func4Factory.Create;
        var funcXFactory = new ComplexDataFuncFactory(func0, func1, func2, func4);
        funcX = funcXFactory.Create;
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Interface0()
    {
        for (var i = 0; i < N; i++)
        {
            factory0.Create(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Interface1()
    {
        for (var i = 0; i < N; i++)
        {
            factory1.Create(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Interface2()
    {
        for (var i = 0; i < N; i++)
        {
            factory2.Create(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Interface4()
    {
        for (var i = 0; i < N; i++)
        {
            factory4.Create(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void InterfaceX()
    {
        for (var i = 0; i < N; i++)
        {
            factoryX.Create(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Func0()
    {
        for (var i = 0; i < N; i++)
        {
            func0(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Func1()
    {
        for (var i = 0; i < N; i++)
        {
            func1(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Func2()
    {
        for (var i = 0; i < N; i++)
        {
            func2(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void Func4()
    {
        for (var i = 0; i < N; i++)
        {
            func4(null);
        }
    }

    [Benchmark(OperationsPerInvoke = N)]
    public void FuncX()
    {
        for (var i = 0; i < N; i++)
        {
            funcX(null);
        }
    }
}

public sealed class Builder
{
    // TODO
}

public interface IResolver
{
    object Resolve(Type type);
}

public interface IFactory
{
    object Create(IResolver resolver);
}

// Manual

public sealed class Data0Factory : IFactory
{
    public object Create(IResolver resolver) => new Data0();
}

public sealed class Data1InterfaceFactory : IFactory
{
    private readonly IFactory factory1;

    public Data1InterfaceFactory(IFactory factory1)
    {
        this.factory1 = factory1;
    }

    public object Create(IResolver resolver) => new Data1((Data0)factory1.Create(resolver));
}

public sealed class Data2InterfaceFactory : IFactory
{
    private readonly IFactory factory1;
    private readonly IFactory factory2;

    public Data2InterfaceFactory(IFactory factory1, IFactory factory2)
    {
        this.factory1 = factory1;
        this.factory2 = factory2;
    }

    public object Create(IResolver resolver) => new Data2((Data0)factory1.Create(resolver), (Data0)factory2.Create(resolver));
}

public sealed class Data4InterfaceFactory : IFactory
{
    private readonly IFactory factory1;
    private readonly IFactory factory2;
    private readonly IFactory factory3;
    private readonly IFactory factory4;

    public Data4InterfaceFactory(IFactory factory1, IFactory factory2, IFactory factory3, IFactory factory4)
    {
        this.factory1 = factory1;
        this.factory2 = factory2;
        this.factory3 = factory3;
        this.factory4 = factory4;
    }

    public object Create(IResolver resolver) => new Data4((Data0)factory1.Create(resolver), (Data0)factory2.Create(resolver), (Data0)factory3.Create(resolver), (Data0)factory4.Create(resolver));
}

public sealed class ComplexDataInterfaceFactory : IFactory
{
    private readonly IFactory factory1;
    private readonly IFactory factory2;
    private readonly IFactory factory3;
    private readonly IFactory factory4;

    public ComplexDataInterfaceFactory(IFactory factory1, IFactory factory2, IFactory factory3, IFactory factory4)
    {
        this.factory1 = factory1;
        this.factory2 = factory2;
        this.factory3 = factory3;
        this.factory4 = factory4;
    }

    public object Create(IResolver resolver) => new ComplexData((Data0)factory1.Create(resolver), (Data1)factory2.Create(resolver), (Data2)factory3.Create(resolver), (Data4)factory4.Create(resolver));
}

public sealed class Data1FuncFactory
{
    private readonly Func<IResolver, object> func1;

    public Data1FuncFactory(Func<IResolver, object> func1)
    {
        this.func1 = func1;
    }

    public object Create(IResolver resolver) => new Data1((Data0)func1(resolver));
}

public sealed class Data2FuncFactory
{
    private readonly Func<IResolver, object> func1;
    private readonly Func<IResolver, object> func2;

    public Data2FuncFactory(Func<IResolver, object> func1, Func<IResolver, object> func2)
    {
        this.func1 = func1;
        this.func2 = func2;
    }

    public object Create(IResolver resolver) => new Data2((Data0)func1(resolver), (Data0)func2(resolver));
}

public sealed class Data4FuncFactory
{
    private readonly Func<IResolver, object> func1;
    private readonly Func<IResolver, object> func2;
    private readonly Func<IResolver, object> func3;
    private readonly Func<IResolver, object> func4;

    public Data4FuncFactory(Func<IResolver, object> func1, Func<IResolver, object> func2, Func<IResolver, object> func3, Func<IResolver, object> func4)
    {
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
        this.func4 = func4;
    }

    public object Create(IResolver resolver) => new Data4((Data0)func1(resolver), (Data0)func2(resolver), (Data0)func3(resolver), (Data0)func4(resolver));
}

public sealed class ComplexDataFuncFactory
{
    private readonly Func<IResolver, object> func1;
    private readonly Func<IResolver, object> func2;
    private readonly Func<IResolver, object> func3;
    private readonly Func<IResolver, object> func4;

    public ComplexDataFuncFactory(Func<IResolver, object> func1, Func<IResolver, object> func2, Func<IResolver, object> func3, Func<IResolver, object> func4)
    {
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
        this.func4 = func4;
    }

    public object Create(IResolver resolver) => new ComplexData((Data0)func1(resolver), (Data1)func2(resolver), (Data2)func3(resolver), (Data4)func4(resolver));
}

// Data0

public class Data0
{
}

public class Data1
{
    public Data0 Value1 { get; }

    public Data1(Data0 value1)
    {
        Value1 = value1;
    }
}

public class Data2
{
    public Data0 Value1 { get; }

    public Data0 Value2 { get; }

    public Data2(Data0 value1, Data0 value2)
    {
        Value1 = value1;
        Value2 = value2;
    }
}

public class Data4
{
    public Data0 Value1 { get; }

    public Data0 Value2 { get; }

    public Data0 Value3 { get; }

    public Data0 Value4 { get; }

    public Data4(Data0 value1, Data0 value2, Data0 value3, Data0 value4)
    {
        Value1 = value1;
        Value2 = value2;
        Value3 = value3;
        Value4 = value4;
    }
}

public class ComplexData
{
    public Data0 Value1 { get; }

    public Data1 Value2 { get; }

    public Data2 Value3 { get; }

    public Data4 Value4 { get; }

    public ComplexData(Data0 value1, Data1 value2, Data2 value3, Data4 value4)
    {
        Value1 = value1;
        Value2 = value2;
        Value3 = value3;
        Value4 = value4;
    }
}
