namespace Smart.Resolver.Builder.Stacks
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class ComponentStack : ObjectStack
    {
        public Type ComponentType { get; }

        public Type ImplementType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="implementType"></param>
        public ComponentStack(Type componentType, Type implementType)
            : base(implementType ?? componentType)
        {
            ComponentType = componentType;
            ImplementType = implementType;
        }
    }
}
