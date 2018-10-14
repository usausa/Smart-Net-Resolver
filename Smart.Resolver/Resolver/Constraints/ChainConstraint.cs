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

        /// <summary>
        ///
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public bool Match(IBindingMetadata metadata)
        {
            return constraints.All(c => c.Match(metadata));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ChainConstraint constraint &&
                (constraints.Length == constraint.constraints.Length))
            {
                for (var i = 0; i < constraints.Length; i++)
                {
                    if (!constraints[i].Equals(constraint.constraints[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hash = 0;
            for (var i = 0; i < constraints.Length; i++)
            {
                hash = hash ^ constraints[i].GetHashCode();
            }

            return hash;
        }
    }
}
