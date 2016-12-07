namespace Smart.Resolver.Metadatas
{
    using System.Collections.Generic;
    using System.Reflection;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public class ConstructorMetadata
    {
        public ConstructorInfo Constructor { get; }

        public IList<ParameterMetadata> Parameters { get; }

        public IList<IConstraint> Constraints { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="parameters"></param>
        /// <param name="constraints"></param>
        public ConstructorMetadata(ConstructorInfo constructor, IList<ParameterMetadata> parameters, IList<IConstraint> constraints)
        {
            Constructor = constructor;
            Parameters = parameters;
            Constraints = constraints;
        }
    }
}
