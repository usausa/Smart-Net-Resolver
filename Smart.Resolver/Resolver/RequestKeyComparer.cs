namespace Smart.Resolver
{
    using System.Collections.Generic;

    public sealed class RequestKeyComparer : IEqualityComparer<RequestKey>
    {
        public bool Equals(RequestKey x, RequestKey y)
        {
            return (x.Type == y.Type) &&
                   (((x.Constraint == null) && (y.Constraint == null)) ||
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
