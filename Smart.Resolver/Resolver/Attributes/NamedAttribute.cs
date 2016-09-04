namespace Smart.Resolver.Attributes
{
    using System;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class NamedAttribute : ConstraintAttribute
    {
        public string Name { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public NamedAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override IConstraint CreateConstraint()
        {
            return new NameConstraint(Name);
        }
    }
}
