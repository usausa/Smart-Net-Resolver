namespace RequestKeyBenchmark.Constraints
{
    using System;

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
            return obj is NameConstraint constraint && Equals(constraint);
        }

        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }
    }
}
