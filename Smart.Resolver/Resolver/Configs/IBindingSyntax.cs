namespace Smart.Resolver.Configs;

using System.Diagnostics.CodeAnalysis;

using Smart.ComponentModel;
using Smart.Resolver.Constraints;
using Smart.Resolver.Parameters;
using Smart.Resolver.Providers;
using Smart.Resolver.Scopes;

#pragma warning disable CA1716
public interface IBindingToSyntax<in T>
{
    IBindingInConstraintWithSyntax ToProvider(Func<ComponentContainer, IProvider> factory);

    IBindingInConstraintWithSyntax ToSelf();

    IBindingInConstraintWithSyntax To<TImplementation>()
        where TImplementation : T;

    IBindingInConstraintWithSyntax To(Type implementationType);

    IBindingInConstraintWithSyntax ToMethod(Func<IResolver, T> factory);

    IBindingInConstraintWithSyntax ToConstant([DisallowNull] T value);
}
#pragma warning restore CA1716

public interface IBindingInSyntax
{
    IBindingConstraintWithSyntax InScope(Func<ComponentContainer, IScope> factory);

    IBindingConstraintWithSyntax InTransientScope();

    IBindingConstraintWithSyntax InSingletonScope();

    IBindingConstraintWithSyntax InContainerScope();
}

public interface IBindingConstraintSyntax
{
    IBindingWithSyntax Constraint(IConstraint constraint);
}

public interface IBindingWithSyntax
{
    IBindingWithSyntax WithMetadata(string key, object? value);

    IBindingWithSyntax WithConstructorArgument(string name, Func<ComponentContainer, IParameter> factory);

    IBindingWithSyntax WithConstructorArgument(string name, object? value);

    IBindingWithSyntax WithConstructorArgument(string name, Func<IResolver, object?> factory);

    IBindingWithSyntax WithPropertyValue(string name, Func<ComponentContainer, IParameter> factory);

    IBindingWithSyntax WithPropertyValue(string name, object? value);

    IBindingWithSyntax WithPropertyValue(string name, Func<IResolver, object?> factory);
}

public interface IBindingConstraintWithSyntax : IBindingConstraintSyntax, IBindingWithSyntax
{
}

public interface IBindingInConstraintWithSyntax : IBindingInSyntax, IBindingConstraintWithSyntax
{
}

public interface IBindingToInConstraintWithSyntax<in T> : IBindingToSyntax<T>, IBindingInConstraintWithSyntax
{
}
