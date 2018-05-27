namespace Smart.Resolver.Metadatas
{
    using System;
    using System.Reflection;

    /// <summary>
    ///
    /// </summary>
    public class OldParameterMetadata
    {
        public ParameterInfo Parameter { get; }

        /// <summary>
        ///
        /// </summary>
        public Type ElementType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="elementType"></param>
        public OldParameterMetadata(ParameterInfo parameter, Type elementType)
        {
            Parameter = parameter;
            ElementType = elementType;
        }
    }
}
