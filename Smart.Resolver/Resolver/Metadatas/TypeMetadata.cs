namespace Smart.Resolver.Metadatas
{
    /// <summary>
    ///
    /// </summary>
    public class TypeMetadata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public ConstructorMetadata[] TargetConstructors { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public PropertyMetadata[] TargetProperties { get; }

        public TypeMetadata(ConstructorMetadata[] targetConstructors, PropertyMetadata[] targetProperties)
        {
            TargetConstructors = targetConstructors;
            TargetProperties = targetProperties;
        }
    }
}
