namespace Smart.Resolver.Builder.Stacks
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IParameterStack
    {
        /// <summary>
        ///
        /// </summary>
        string Name { get; }

        /// <summary>
        ///
        /// </summary>
        Type ParameterType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        void SetValue(object value);
    }
}
