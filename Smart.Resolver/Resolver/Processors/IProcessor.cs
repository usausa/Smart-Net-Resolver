namespace Smart.Resolver.Processors;

public interface IProcessor
{
    int Order { get; }

    Action<IResolver, object>? CreateProcessor(Type type);
}
