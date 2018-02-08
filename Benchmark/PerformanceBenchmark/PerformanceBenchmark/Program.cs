namespace PerformanceBenchmark
{
    using BenchmarkDotNet.Running;

    using PerformanceBenchmark.Benchmarks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<SmartDefaultBenchmark>();
            BenchmarkRunner.Run<SmartUseEmitBenchmark>();
            BenchmarkRunner.Run<SmartUseReflectionBenchmark>();
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
