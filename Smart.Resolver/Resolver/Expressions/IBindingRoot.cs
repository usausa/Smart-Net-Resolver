namespace Smart.Resolver.Expressions;

using System.Diagnostics.CodeAnalysis;

public interface IBindingRoot
{
    IBindingToSyntax<T> Bind<[DynamicallyAccessedMembers(LinkerFlags.Binding)] T>();

    IBindingToSyntax<object> Bind(
        [DynamicallyAccessedMembers(LinkerFlags.Binding)] Type type);
}
