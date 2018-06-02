namespace Smart.Resolver.Benchmark
{
    using BenchmarkDotNet.Running;

    using Smart.Resolver.Benchmark.Benchmarks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
