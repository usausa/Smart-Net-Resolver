namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Providers;

    public sealed class DependencyServiceMissingHandler : IMissingHandler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if (!type.IsInterface)
            {
                return Enumerable.Empty<IBinding>();
            }

            return new[]
            {
                new Binding(type, new DependencyServiceProvider(type))
            };
        }
    }
}
