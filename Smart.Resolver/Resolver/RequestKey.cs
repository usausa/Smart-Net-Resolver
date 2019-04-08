namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Constraints;

    internal sealed class RequestKey
    {
        public Type Type { get; }

        public IConstraint Constraint { get; }

        public RequestKey(Type type, IConstraint constraint)
        {
            Type = type;
            Constraint = constraint;
        }
    }
}
