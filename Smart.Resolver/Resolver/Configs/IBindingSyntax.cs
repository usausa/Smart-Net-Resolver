namespace Smart.Resolver.Configs
{
    using System;
    using Smart.ComponentModel;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    public interface IBindingToSyntax<in T>
    {
        IBindingInNamedWithSyntax ToProvider(Func<IComponentContainer, IProvider> factory);

        IBindingInNamedWithSyntax ToSelf();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Ignore")]
        IBindingInNamedWithSyntax To<TImplementation>()
            where TImplementation : T;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Ignore")]
        IBindingInNamedWithSyntax To(Type implementationType);

        IBindingInNamedWithSyntax ToMethod(Func<IResolver, T> factory);

        IBindingInNamedWithSyntax ToConstant(T value);
    }

    public interface IBindingInSyntax
    {
        IBindingNamedWithSyntax InScope(Func<IComponentContainer, IScope> factory);

        IBindingNamedWithSyntax InTransientScope();

        IBindingNamedWithSyntax InSingletonScope();

        IBindingNamedWithSyntax InContainerScope();
    }

    public interface IBindingNamedSyntax
    {
        IBindingWithSyntax Named(string name);
    }

    public interface IBindingWithSyntax
    {
        IBindingWithSyntax WithMetadata(string key, object value);

        IBindingWithSyntax WithConstructorArgument(string name, Func<IComponentContainer, IParameter> factory);

        IBindingWithSyntax WithConstructorArgument(string name, object value);

        IBindingWithSyntax WithConstructorArgument(string name, Func<IResolver, object> factory);

        IBindingWithSyntax WithPropertyValue(string name, Func<IComponentContainer, IParameter> factory);

        IBindingWithSyntax WithPropertyValue(string name, object value);

        IBindingWithSyntax WithPropertyValue(string name, Func<IResolver, object> factory);
    }

    public interface IBindingToInNamedWithSyntax<in T> : IBindingToSyntax<T>, IBindingInNamedWithSyntax
    {
    }

    public interface IBindingInNamedWithSyntax : IBindingInSyntax, IBindingNamedWithSyntax
    {
    }

    public interface IBindingNamedWithSyntax : IBindingNamedSyntax, IBindingWithSyntax
    {
    }
}
