namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public sealed class AssignableMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreTypes;

        /// <summary>
        ///
        /// </summary>
        public AssignableMissingHandler()
            : this(Type.EmptyTypes)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreTypes"></param>
        public AssignableMissingHandler(IEnumerable<Type> ignoreTypes)
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
            if (ignoreTypes.Contains(type))
            {
                return Enumerable.Empty<IBinding>();
            }

            return table.EnumBindings().Where(x => type.IsAssignableFrom(x.Provider.TargetType));
        }
    }
}
