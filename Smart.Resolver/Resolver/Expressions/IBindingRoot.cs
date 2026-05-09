namespace Smart.Resolver.Expressions;

public interface IBindingRoot
{
    IBindingToSyntax<T> Bind<T>();

    IBindingToSyntax<object> Bind(Type type);
}
