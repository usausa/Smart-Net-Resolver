namespace PerformanceBenchmark
{
    using BenchmarkDotNet.Running;

    using PerformanceBenchmark.Benchmarks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
