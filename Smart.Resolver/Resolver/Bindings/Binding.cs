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
    public class Binding : IBinding
    {
        private Dictionary<string, IParameter> constructorArgument;

        private Dictionary<string, IParameter> propertyValue;

        /// <summary>
        ///
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///
        /// </summary>
        public IBindingMetadata Metadata { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IProvider Provider { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IScope Scope { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="metadata"></param>
        public Binding(Type type, IBindingMetadata metadata)
        {
            Type = type;
            Metadata = metadata;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        public void AddConstructorArgument(string name, IParameter parameter)
        {
            if (constructorArgument == null)
            {
                constructorArgument = new Dictionary<string, IParameter>();
            }

            constructorArgument[name] = parameter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IParameter GetConstructorArgument(string name)
        {
            IParameter parameter;
            return (constructorArgument != null) && constructorArgument.TryGetValue(name, out parameter)
                ? parameter
                : null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        public void AddPropertyValue(string name, IParameter parameter)
        {
            if (propertyValue == null)
            {
                propertyValue = new Dictionary<string, IParameter>();
            }

            propertyValue[name] = parameter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IParameter GetPropertyValue(string name)
        {
            IParameter parameter;
            return (propertyValue != null) && propertyValue.TryGetValue(name, out parameter)
                ? parameter
                : null;
        }
    }
}
