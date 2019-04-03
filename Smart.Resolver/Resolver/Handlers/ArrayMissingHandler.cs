namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Helpers;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public class ArrayMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreElementTypes;

        /// <summary>
        ///
        /// </summary>
        public ArrayMissingHandler()
            : this(Type.EmptyTypes)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreElementTypes"></param>
        public ArrayMissingHandler(IEnumerable<Type> ignoreElementTypes)
        {
            this.ignoreElementTypes = new HashSet<Type>(ignoreElementTypes);
        }

        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            var elementType = TypeHelper.GetEnumerableElementType(type);
            if (elementType is null)
            {
                return Enumerable.Empty<IBinding>();
            }

            if (ignoreElementTypes.Contains(elementType))
            {
                return Enumerable.Empty<IBinding>();
            }

            var bindings = table.FindBindings(elementType);

            // hack for singleton
            var useSingleton = bindings.Length > 0 && bindings.All(b => b.Scope is SingletonScope);
            return new[]
            {
                new Binding(
                    type,
                    new BindingArrayProvider(type, elementType, components, bindings),
                    useSingleton ? new SingletonScope(components) : null,
                    null,
                    null,
                    null)
            };
        }
    }
}
