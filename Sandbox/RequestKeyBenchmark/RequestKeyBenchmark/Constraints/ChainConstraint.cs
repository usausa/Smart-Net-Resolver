namespace RequestKeyBenchmark.Constraints
{
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

        public bool Equals(IConstraint other)
        {
            if (other is ChainConstraint constraint &&
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

        public override bool Equals(object obj)
        {
            return obj is ChainConstraint constraint && Equals(constraint);
        }

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
