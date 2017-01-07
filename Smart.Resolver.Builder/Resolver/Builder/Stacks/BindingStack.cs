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
        private IDictionary<string, object> metadatas;

        private IDictionary<string, Func<IComponentContainer, IParameter>> constructorArtumentFactories;

        private IDictionary<string, Func<IComponentContainer, IParameter>> propertyValueFactories;

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
        public IDictionary<string, object> Metadatas => metadatas;

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, Func<IComponentContainer, IParameter>> ConstructorArgumentFactories => constructorArtumentFactories;

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, Func<IComponentContainer, IParameter>> PropertyValueFactories => propertyValueFactories;

        /// <summary>
        ///
        /// </summary>
        /// <param name="componentType"></param>
        public BindingStack(Type componentType)
        {
            ComponentType = componentType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddMetadata(string name, object value)
        {
            if (metadatas == null)
            {
                metadatas = new Dictionary<string, object>();
            }

            metadatas[name] = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factory"></param>
        public void AddConstructorArgument(string name, Func<IComponentContainer, IParameter> factory)
        {
            if (constructorArtumentFactories == null)
            {
                constructorArtumentFactories = new Dictionary<string, Func<IComponentContainer, IParameter>>();
            }

            constructorArtumentFactories[name] = factory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factory"></param>
        public void AddPropertyValue(string name, Func<IComponentContainer, IParameter> factory)
        {
            if (propertyValueFactories == null)
            {
                propertyValueFactories = new Dictionary<string, Func<IComponentContainer, IParameter>>();
            }

            propertyValueFactories[name] = factory;
        }
    }
}
