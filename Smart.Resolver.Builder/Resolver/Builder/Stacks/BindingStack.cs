namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Builder.Scopes;
    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class BindingStack
    {
        /// <summary>
        ///
        /// </summary>
        public Type ComponentType { get; }

        /// <summary>
        ///
        /// </summary>
        public Func<IComponentContainer, IProvider> ProviderFactory { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IScopeProcessor ScopeProcessor { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, object> Metadatas { get; } = new Dictionary<string, object>();

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, Func<IComponentContainer, IParameter>> ConstructorArgumentFactories { get; } = new Dictionary<string, Func<IComponentContainer, IParameter>>();

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, Func<IComponentContainer, IParameter>> PropertyValueFactories { get; } = new Dictionary<string, Func<IComponentContainer, IParameter>>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="componentType"></param>
        public BindingStack(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
