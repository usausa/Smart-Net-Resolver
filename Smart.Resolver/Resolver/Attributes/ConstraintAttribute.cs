namespace Smart.Resolver.Attributes
{
    using System;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public abstract class ConstraintAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract IConstraint CreateConstraint();
    }
}
