namespace Smart.Resolver
{
    using System.Collections.Generic;

    internal sealed class RequestKeyComparer : IEqualityComparer<RequestKey>
    {
        public static RequestKeyComparer Default { get; } = new RequestKeyComparer();

        public bool Equals(RequestKey x, RequestKey y)
        {
            // ReSharper disable PossibleNullReferenceException
            return (x.Type == y.Type) &&
                   // ReSharper restore PossibleNullReferenceException
                   (((x.Constraint == null) && (y.Constraint == null)) ||
                    ((x.Constraint != null) && (y.Constraint != null) && x.Constraint.Equals(y.Constraint)));
        }

        public int GetHashCode(RequestKey obj)
        {
            return obj.GetHashCode();
        }
    }
}
