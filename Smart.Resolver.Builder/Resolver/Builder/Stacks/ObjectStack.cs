namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class ObjectStack
    {
        /// <summary>
        ///
        /// </summary>
        public IList<KeyValuePair<string, object>> ConstructorArguments { get; } = new List<KeyValuePair<string, object>>();

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, object> PropertyValues { get; } = new Dictionary<string, object>();

        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetType"></param>
        public ObjectStack(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
