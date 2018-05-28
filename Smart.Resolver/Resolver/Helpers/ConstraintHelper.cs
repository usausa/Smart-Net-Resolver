namespace Smart.Resolver.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Resolver.Attributes;
    using Smart.Resolver.Constraints;

    public static class ConstraintHelper
    {
        public static IConstraint CreateConstraint(IEnumerable<ConstraintAttribute> attributes)
        {
            var constraints = attributes
                .Select(a => a.CreateConstraint())
                .ToArray();

            if (constraints.Length == 0)
            {
                return null;
            }

            if (constraints.Length == 1)
            {
                return constraints[0];
            }

            return new ChainConstraint(constraints);
        }
    }
}