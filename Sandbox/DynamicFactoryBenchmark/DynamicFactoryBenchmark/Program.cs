namespace DynamicFactoryBenchmark
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;

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
            Add(Job.MediumRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private Func<Data> byNull;
        private Func<Data> byObject;
        private Func<Data> byStruct;

        [GlobalSetup]
        public void Setup()
        {
            var ci = typeof(Data).GetConstructor(Type.EmptyTypes);
            byNull = Builder.Create<Data>(ci, null);
            byObject = Builder.Create<Data>(ci, new object());
            byStruct = Builder.Create<Data>(ci, new Factory());
        }

        [Benchmark]
        public Data ByNull() => byNull();

        [Benchmark]
        public Data ByObject() => byObject();

        [Benchmark]
        public Data ByStruct() => byStruct();
    }

    public class Data
    {
    }

    public struct Factory
    {
    }

    public static class Builder
    {
        public static Func<T> Create<T>(ConstructorInfo ci, object target)
        {
            var dynamic = new DynamicMethod(string.Empty, typeof(T), Type.EmptyTypes, true);
            var il = dynamic.GetILGenerator();

            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Ret);

            return (Func<T>)dynamic.CreateDelegate(typeof(Func<T>), null);
        }
    }
}
