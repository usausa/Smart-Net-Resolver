namespace Smart.Resolver.Constraints
{
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IConstraint
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        bool Match(IBindingMetadata metadata);
    }
}
