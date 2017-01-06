namespace Smart.Resolver.Builder.Stacks
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public interface IBindingStack
    {
        /// <summary>
        ///
        /// </summary>
        Type ComponentType { get; set; }

        /// <summary>
        ///
        /// </summary>
        Type ImplementationType { get; set; }

        /// <summary>
        ///
        /// </summary>
        Func<IComponentContainer, IProvider> ProviderFactory { get; set; }

        /// <summary>
        ///
        /// </summary>
        Func<IComponentContainer, IScope> ScopeFactory { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void AddMetadata(string name, object value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        void AddConstructorArgument(string name, IParameter parameter);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        void AddPropertyValue(string name, IParameter parameter);
    }
}
