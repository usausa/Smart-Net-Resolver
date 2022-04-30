namespace Smart.Resolver.Injectors;

using Smart.Resolver.Bindings;

public interface IInjector
{
    Action<IResolver, object>? CreateInjector(Type type, Binding binding);
}
