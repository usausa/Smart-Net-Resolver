namespace RequestKeyBenchmark.Constraints
{
    /// <summary>
    ///
    /// </summary>
    public sealed class NameConstraint : IConstraint
    {
        private readonly string name;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public NameConstraint(string name)
        {
            this.name = name;
        }

        public bool Equals(IConstraint other)
        {
            return other is NameConstraint constraint && string.Equals(name, constraint.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NameConstraint constraint && Equals(constraint);
        }

        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }
    }
}
