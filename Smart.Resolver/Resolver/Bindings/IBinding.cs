namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        ///
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///
        /// </summary>
        IBindingMetadata Metadata { get; }

        /// <summary>
        ///
        /// </summary>
        IProvider Provider { get; }

        /// <summary>
        ///
        /// </summary>
        IScope Scope { get; }

        /// <summary>
        ///
        /// </summary>
        ParameterMap ConstructorArguments { get; }

        /// <summary>
        ///
        /// </summary>
        ParameterMap PropertyValues { get; }
    }
}
