namespace Smart.Resolver.Configs;

public interface IBindingRoot
{
    IBindingToSyntax<T> Bind<T>();

    IBindingToSyntax<object> Bind(Type type);
}
