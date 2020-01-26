namespace Smart.Resolver.Constraints
{
    using System.Linq;

    using Smart.Resolver.Bindings;

    public sealed class ChainConstraint : IConstraint
    {
        private readonly IConstraint[] constraints;

        public ChainConstraint(params IConstraint[] constraints)
        {
            this.constraints = constraints;
        }

        public bool Match(IBindingMetadata metadata)
        {
            return constraints.All(c => c.Match(metadata));
        }

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

        public override int GetHashCode()
        {
            var hash = 0;
            for (var i = 0; i < constraints.Length; i++)
            {
                hash ^= constraints[i].GetHashCode();
            }

            return hash;
        }
    }
}
