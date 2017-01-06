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
        public string Name { get; }

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
        /// <param name="name"></param>
        /// <param name="parameterType"></param>
        public ParameterStack(string name, Type parameterType)
        {
            Name = name;
            ParameterType = parameterType;
        }
    }
}
