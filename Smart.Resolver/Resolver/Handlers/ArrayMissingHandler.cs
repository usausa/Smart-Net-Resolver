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

    public class ArrayMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreElementTypes;

        public ArrayMissingHandler()
            : this(Type.EmptyTypes)
        {
        }

        public ArrayMissingHandler(IEnumerable<Type> ignoreElementTypes)
        {
            this.ignoreElementTypes = new HashSet<Type>(ignoreElementTypes);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Factory")]
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
