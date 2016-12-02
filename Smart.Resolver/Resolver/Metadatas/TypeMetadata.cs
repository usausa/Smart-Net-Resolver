namespace Smart.Resolver.Metadatas
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class TypeMetadata
    {
        public IList<ConstructorMetadata> TargetConstructors { get; }

        public IList<PropertyMetadata> TargetProperties { get; }

        public TypeMetadata(IList<ConstructorMetadata> targetConstructors, IList<PropertyMetadata> targetProperties)
        {
            TargetConstructors = targetConstructors;
            TargetProperties = targetProperties;
        }
    }
}
