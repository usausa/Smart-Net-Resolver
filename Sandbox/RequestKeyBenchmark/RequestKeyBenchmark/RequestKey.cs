namespace RequestKeyBenchmark
{
    using System;
    using System.Collections.Generic;

    using RequestKeyBenchmark.Constraints;


    public class RequestKey
    {
        public Type Type { get; }

        public IConstraint Constraint { get; }

        public RequestKey(Type type, IConstraint constraint)
        {
            Type = type;
            Constraint = constraint;
        }
    }

    public sealed class RequestKeyComparer : IEqualityComparer<RequestKey>
    {
        public bool Equals(RequestKey x, RequestKey y)
        {
            return ((x.Type == y.Type) &&
                    (((x.Constraint == null) && (y.Constraint == null)) ||
                     ((x.Constraint != null) && (y.Constraint != null) && x.Constraint.Equals(y.Constraint))));
        }

        public int GetHashCode(RequestKey obj)
        {
            var hash = obj.Type.GetHashCode();
            if (obj.Constraint != null)
            {
                hash = hash ^ obj.Constraint.GetHashCode();
            }
            return hash;
        }
    }
}
