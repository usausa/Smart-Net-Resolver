namespace Smart.Resolver.Metadatas
{
    using System.Reflection;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public class OldConstructorMetadata
    {
        public ConstructorInfo Constructor { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public OldParameterMetadata[] Parameters { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public IConstraint[] Constraints { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="parameters"></param>
        /// <param name="constraints"></param>
        public OldConstructorMetadata(ConstructorInfo constructor, OldParameterMetadata[] parameters, IConstraint[] constraints)
        {
            Constructor = constructor;
            Parameters = parameters;
            Constraints = constraints;
        }
    }
}
