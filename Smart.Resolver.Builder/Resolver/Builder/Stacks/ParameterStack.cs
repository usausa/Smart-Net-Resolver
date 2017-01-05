namespace Smart.Resolver.Builder.Stacks
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class ParameterStack
    {
        /// <summary>
        ///
        /// </summary>
        public Type ParameterType { get; }

        /// <summary>
        ///
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterType"></param>
        public ParameterStack(Type parameterType)
        {
            ParameterType = parameterType;
        }
    }
}
