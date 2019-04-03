namespace Smart.Resolver
{
    using System.Collections.Generic;

    internal sealed class RequestKeyComparer : IEqualityComparer<RequestKey>
    {
        public static RequestKeyComparer Default { get; } = new RequestKeyComparer();

        public bool Equals(RequestKey x, RequestKey y)
        {
            return (x.Type == y.Type) &&
                   ((x.Constraint is null && y.Constraint is null) ||
                    ((x.Constraint != null) && (y.Constraint != null) && x.Constraint.Equals(y.Constraint)));
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
