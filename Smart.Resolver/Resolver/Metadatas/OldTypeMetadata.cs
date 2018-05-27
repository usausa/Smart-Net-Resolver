namespace Smart.Resolver.Metadatas
{
    /// <summary>
    ///
    /// </summary>
    public class OldTypeMetadata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public OldConstructorMetadata[] TargetConstructors { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public OldPropertyMetadata[] TargetProperties { get; }

        public OldTypeMetadata(OldConstructorMetadata[] targetConstructors, OldPropertyMetadata[] targetProperties)
        {
            TargetConstructors = targetConstructors;
            TargetProperties = targetProperties;
        }
    }
}
