namespace Smart.Resolver.Benchmark.Classes;

public interface IScopedService
{
    void DoSomething();
}

public sealed class ScopedService : IScopedService
{
    public void DoSomething()
    {
    }
}
