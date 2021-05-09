namespace Smart.Resolver.Constraints
{
    using Smart.Resolver.Bindings;

    public sealed class ChainConstraint : IConstraint
    {
        private readonly IConstraint[] constraints;

        public ChainConstraint(params IConstraint[] constraints)
        {
            this.constraints = constraints;
        }

        public bool Match(BindingMetadata metadata)
        {
            var constraintsLocal = constraints;
            for (var i = 0; i < constraintsLocal.Length; i++)
            {
                if (!constraintsLocal[i].Match(metadata))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ChainConstraint constraint)
            {
                var constraintsLocal = constraints;
                var constraintsOther = constraint.constraints;
                if (constraintsLocal.Length == constraintsOther.Length)
                {
                    for (var i = 0; i < constraintsLocal.Length; i++)
                    {
                        if (!constraintsLocal[i].Equals(constraintsOther[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hash = 0;
            var constraintsLocal = constraints;
            for (var i = 0; i < constraintsLocal.Length; i++)
            {
                hash ^= constraintsLocal[i].GetHashCode();
            }

            return hash;
        }
    }
}
