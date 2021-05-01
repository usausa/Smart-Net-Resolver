namespace Smart.Resolver.Constraints
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class NameConstraint : IConstraint
    {
        private readonly string name;

        public NameConstraint(string name)
        {
            this.name = name;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public bool Match(BindingMetadata metadata) => name == metadata.Name;

        public override bool Equals(object? obj)
        {
            return obj is NameConstraint constraint && name == constraint.name;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Ignore")]
        public override int GetHashCode() => name.GetHashCode();
    }
}
