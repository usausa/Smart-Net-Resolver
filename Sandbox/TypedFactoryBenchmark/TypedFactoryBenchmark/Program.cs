namespace TypedFactoryBenchmark
{
    using System;
    using System.Reflection;

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
            BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);
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

    public class FuncResolver
    {
        // TODO
    }

    public interface IFactory
    {
        object Create();
    }

    public interface IFactory<out T> : IFactory
    {
        T CreateTyped();
    }

    public class TypedFactoryResolver
    {
        // TODO
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        // TODO
    }
}
