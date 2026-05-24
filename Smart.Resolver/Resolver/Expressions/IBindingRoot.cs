namespace Smart.Resolver.Expressions;

using System.Diagnostics.CodeAnalysis;

public interface IBindingRoot
{
    IBindingToSyntax<T> Bind<T>();

    IBindingToSyntax<object> Bind(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type type);
}
