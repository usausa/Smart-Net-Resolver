namespace Smart.Resolver.Constraints
{
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
    }
}
