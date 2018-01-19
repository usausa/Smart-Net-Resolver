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
        private static readonly Type StringType = typeof(string);

        private static readonly Type DelegateType = typeof(Delegate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <param name="table"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if (type.IsInterface || type.IsAbstract || type.IsValueType || (type == StringType) ||
                DelegateType.IsAssignableFrom(type) || type.ContainsGenericParameters)
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
