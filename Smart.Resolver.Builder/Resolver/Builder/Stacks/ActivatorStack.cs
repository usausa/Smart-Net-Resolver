namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Builder.Handlers;

    /// <summary>
    ///
    /// </summary>
    public class ActivatorStack
    {
        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        public ConstructorArguments ConstructorArguments { get; } = new ConstructorArguments();

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, object> PropertyValues { get; } = new Dictionary<string, object>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetType"></param>
        public ActivatorStack(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
