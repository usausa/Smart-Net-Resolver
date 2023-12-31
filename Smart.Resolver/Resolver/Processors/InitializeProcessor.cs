namespace Smart.Resolver.Processors;

public sealed class InitializeProcessor : IProcessor
{
    public int Order { get; }

    public InitializeProcessor()
        : this(Int32.MinValue)
    {
    }

    public InitializeProcessor(int order)
    {
        Order = order;
    }

    public Action<IResolver, object>? CreateProcessor(Type type)
    {
        if (!typeof(IInitializable).IsAssignableFrom(type))
        {
            return null;
        }

        return static (_, x) => ((IInitializable)x).Initialize();
    }
}
