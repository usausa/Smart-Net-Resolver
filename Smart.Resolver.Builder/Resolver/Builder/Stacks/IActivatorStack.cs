namespace Smart.Resolver.Builder.Stacks
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IActivatorStack
    {
        /// <summary>
        /// /
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void AddConstructorArgument(string name, object value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetPropertyValue(string name, object value);
    }
}
