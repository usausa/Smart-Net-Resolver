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

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is RequestKey other)
            {
                return Type == other.Type && Constraint.Equals(other.Constraint);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            hash = hash ^ Constraint.GetHashCode();
            return hash;
        }
    }
}
