namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class SelfMissingHandler : IMissingHandler
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
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsInterface || typeInfo.IsAbstract || typeInfo.IsValueType || (type == StringType) ||
                DelegateType.IsAssignableFrom(type) || typeInfo.ContainsGenericParameters)
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
