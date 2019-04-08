namespace Smart.Resolver.Attributes
{
    using System;

    using Smart.Resolver.Constraints;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class NamedAttribute : ConstraintAttribute
    {
        public string Name { get; }

        public NamedAttribute(string name)
        {
            Name = name;
        }

        public override IConstraint CreateConstraint()
        {
            return new NameConstraint(Name);
        }
    }
}
