namespace Smart.Resolver.Constraints
{
    using System;

    using Smart.Resolver.Bindings;

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public bool Match(IBindingMetadata metadata)
        {
            return name == metadata.Name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is NameConstraint constraint && String.Equals(name, constraint.name, StringComparison.Ordinal);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return name?.GetHashCode() ?? 0;
        }
    }
}
