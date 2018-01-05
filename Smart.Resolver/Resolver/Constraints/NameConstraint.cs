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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public bool Match(IBindingMetadata metadata)
        {
            return name == metadata.Name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IConstraint other)
        {
            return other is NameConstraint constraint && String.Equals(name, constraint.name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is NameConstraint constraint && Equals(constraint);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }
    }
}
