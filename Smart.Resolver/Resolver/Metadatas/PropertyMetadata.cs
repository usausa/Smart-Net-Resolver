namespace Smart.Resolver.Metadatas
{
    using Smart.Reflection;
    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public class PropertyMetadata
    {
        public IAccessor Accessor { get; }

        public IConstraint Constraint { get; }

        public PropertyMetadata(IAccessor accessor, IConstraint constraint)
        {
            Accessor = accessor;
            Constraint = constraint;
        }
    }
}
