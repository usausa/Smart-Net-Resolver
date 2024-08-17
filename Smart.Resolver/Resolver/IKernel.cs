namespace Smart.Resolver;

public interface IKernel
{
    bool TryResolveFactory(Type type, object? key, out Func<IResolver, object> factory);

    bool TryResolveFactories(Type type, object? key, out Func<IResolver, object>[] factories);
}
