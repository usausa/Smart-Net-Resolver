namespace Smart.Resolver.Constraints
{
    using System.Linq;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public sealed class ChainConstraint : IConstraint
    {
        private readonly IConstraint[] constraints;

        /// <summary>
        ///
        /// </summary>
        /// <param name="constraints"></param>
        public ChainConstraint(params IConstraint[] constraints)
        {
            this.constraints = constraints;
        }

        public bool Match(IBindingMetadata metadata)
        {
            return constraints.All(c => c.Match(metadata));
        }
    }
}
