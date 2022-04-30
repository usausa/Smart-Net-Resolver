namespace Smart.Resolver.Benchmark.Classes;

using System.Diagnostics.CodeAnalysis;

public interface IGenericObject<T>
{
    T Value { get; set; }
}

public class GenericObject<T> : IGenericObject<T>
{
    public T Value { get; set; } = default!;
}
