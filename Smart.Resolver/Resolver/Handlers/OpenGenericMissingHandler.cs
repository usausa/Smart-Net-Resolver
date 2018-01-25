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
    public sealed class OpenGenericMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreTypes;

        /// <summary>
        ///
        /// </summary>
        public OpenGenericMissingHandler()
            : this(Type.EmptyTypes)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreTypes"></param>
        public OpenGenericMissingHandler(IEnumerable<Type> ignoreTypes)
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if (!type.IsGenericType || ignoreTypes.Contains(type))
            {
                return Enumerable.Empty<IBinding>();
            }

            return table.FindBindings(type.GetGenericTypeDefinition())
                .Select(b => new Binding(
                    type,
                    new StandardProvider(b.Provider.TargetType.MakeGenericType(type.GenericTypeArguments), components),
                    b.Scope?.Copy(components),
                    b.Metadata,
                    b.ConstructorArguments,
                    b.PropertyValues));
        }
    }
}
