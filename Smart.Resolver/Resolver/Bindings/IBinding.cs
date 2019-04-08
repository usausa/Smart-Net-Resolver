namespace Smart.Resolver.Bindings
{
    using System;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    public interface IBinding
    {
        Type Type { get; }

        IBindingMetadata Metadata { get; }

        IProvider Provider { get; }

        IScope Scope { get; }

        ParameterMap ConstructorArguments { get; }

        ParameterMap PropertyValues { get; }
    }
}
