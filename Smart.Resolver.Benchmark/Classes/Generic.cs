namespace Smart.Resolver.Benchmark.Classes
{
    public interface IGenericObject<T>
    {
        T Value { get; set; }
    }

    public class GenericObject<T> : IGenericObject<T>
    {
        public T Value { get; set; }
    }
}
