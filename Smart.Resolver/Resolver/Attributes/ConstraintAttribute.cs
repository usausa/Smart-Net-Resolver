namespace Smart.Resolver.Attributes
{
    using System;

    using Smart.Resolver.Constraints;

    public abstract class ConstraintAttribute : Attribute
    {
        public abstract IConstraint CreateConstraint();
    }
}
