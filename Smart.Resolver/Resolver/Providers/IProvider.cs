namespace Smart.Resolver.Providers;

using Smart.Resolver.Bindings;

public interface IProvider
{
    Type TargetType { get; }

    Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding);
}
