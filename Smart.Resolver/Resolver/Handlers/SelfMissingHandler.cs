namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public sealed class SelfMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreTypes;

        /// <summary>
        ///
        /// </summary>
        public SelfMissingHandler()
            : this(new[] { typeof(string), typeof(Delegate) })
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreTypes"></param>
        public SelfMissingHandler(IEnumerable<Type> ignoreTypes)
        {
            this.ignoreTypes = new HashSet<Type>(ignoreTypes);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <param name="table"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if (type.IsInterface || type.IsAbstract || type.IsValueType || type.ContainsGenericParameters ||
                ignoreTypes.Contains(type))
            {
                return Enumerable.Empty<IBinding>();
            }

            return new[]
            {
                new Binding(type, new StandardProvider(type, components))
            };
        }
    }
}
