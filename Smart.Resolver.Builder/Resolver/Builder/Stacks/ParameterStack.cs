namespace Smart.Resolver.Builder.Stacks
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class ParameterStack : IParameterStack
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
        public object Value { get; private set; }

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            Value = value;
        }
    }
}
