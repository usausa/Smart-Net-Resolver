namespace Smart.Resolver.Bindings
{
    using System;

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
        /// <param name="name"></param>
        /// <returns></returns>
        IParameter GetConstructorArgument(string name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IParameter GetPropertyValue(string name);
    }
}
