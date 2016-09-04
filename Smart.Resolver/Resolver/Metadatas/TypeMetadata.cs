namespace Smart.Resolver.Metadatas
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class TypeMetadata
    {
        public ConstructorMetadata TargetConstructor { get; }

        public IList<PropertyMetadata> TargetProperties { get; }

        public TypeMetadata(ConstructorMetadata targetConstructor, IList<PropertyMetadata> targetProperties)
        {
            TargetConstructor = targetConstructor;
            TargetProperties = targetProperties;
        }
    }
}
