namespace Smart.Resolver.Benchmark.Classes;

public interface IGenericObject<T>
{
    T Value { get; set; }
}

public sealed class GenericObject<T> : IGenericObject<T>
{
    public T Value { get; set; } = default!;
}
