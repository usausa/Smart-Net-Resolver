namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class ActivatorStack
    {
        private IList<KeyValuePair<string, object>> constructorArguments;

        private IDictionary<string, object> propertyValues;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        public ICollection<KeyValuePair<string, object>> ConstructorArguments => constructorArguments;

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, object> PropertyValues => propertyValues;

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetType"></param>
        public ActivatorStack(Type targetType)
        {
            TargetType = targetType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddConstructorArgument(string name, object value)
        {
            if (constructorArguments == null)
            {
                constructorArguments = new List<KeyValuePair<string, object>>();
            }

            constructorArguments.Add(new KeyValuePair<string, object>(name, value));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetPropertyValue(string name, object value)
        {
            if (propertyValues == null)
            {
                propertyValues = new Dictionary<string, object>();
            }

            propertyValues[name] = value;
        }
    }
}
