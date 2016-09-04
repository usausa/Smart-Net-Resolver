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

        public IList<IConstraint> Constraints { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="constraints"></param>
        public ConstructorMetadata(ConstructorInfo constructor, IList<IConstraint> constraints)
        {
            Constructor = constructor;
            Constraints = constraints;
        }
    }
}
