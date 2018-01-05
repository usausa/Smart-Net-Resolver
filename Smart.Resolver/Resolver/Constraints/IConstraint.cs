namespace Smart.Resolver.Constraints
{
    using System;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IConstraint : IEquatable<IConstraint>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        bool Match(IBindingMetadata metadata);
    }
}
