namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    internal sealed class RequestKey
    {
        public Type Type { get; }

        public IConstraint Constraint { get; }

        public RequestKey(Type type, IConstraint constraint)
        {
            Type = type;
            Constraint = constraint;
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            hash = hash ^ Constraint.GetHashCode();
            return hash;
        }
    }
}
