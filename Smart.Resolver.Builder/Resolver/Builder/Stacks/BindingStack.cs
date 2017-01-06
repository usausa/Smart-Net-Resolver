namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Builder.Scopes;
    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public class BindingStack
    {
        private IDictionary<string, object> metadatas;

        private IDictionary<string, IParameter> constructorArtuments;

        private IDictionary<string, IParameter> propertyValues;

        /// <summary>
        ///
        /// </summary>
        public Type ComponentType { get; }

        /// <summary>
        ///
        /// </summary>
        public Type ImplementationType { get; set; }

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
        public IDictionary<string, IParameter> ConstructorArguments => constructorArtuments;

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, IParameter> PropertyValues => propertyValues;

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
        /// <param name="parameter"></param>
        public void AddConstructorArgument(string name, IParameter parameter)
        {
            if (constructorArtuments == null)
            {
                constructorArtuments = new Dictionary<string, IParameter>();
            }

            constructorArtuments[name] = parameter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        public void AddPropertyValue(string name, IParameter parameter)
        {
            if (propertyValues == null)
            {
                propertyValues = new Dictionary<string, IParameter>();
            }

            propertyValues[name] = parameter;
        }
    }
}
