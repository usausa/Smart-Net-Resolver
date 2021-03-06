namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Providers;

    public sealed class SelfMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreTypes;

        public SelfMissingHandler()
            : this(new[] { typeof(string), typeof(Delegate) })
        {
        }

        public SelfMissingHandler(IEnumerable<Type> ignoreTypes)
        {
            this.ignoreTypes = new HashSet<Type>(ignoreTypes);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
        {
            if (type.IsInterface || type.IsAbstract || type.IsValueType || type.ContainsGenericParameters ||
                ignoreTypes.Contains(type))
            {
                return Enumerable.Empty<Binding>();
            }

            return new[]
            {
                new Binding(type, new StandardProvider(type, components))
            };
        }
    }
}
