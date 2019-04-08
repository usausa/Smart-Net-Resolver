namespace Smart.Resolver.Configs
{
    using System;

    public interface IBindingRoot
    {
        IBindingToSyntax<T> Bind<T>();

        IBindingToSyntax<object> Bind(Type type);
    }
}
