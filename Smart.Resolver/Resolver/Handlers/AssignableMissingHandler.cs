namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public sealed class AssignableMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> ignoreTypes;

        public AssignableMissingHandler()
            : this(Type.EmptyTypes)
        {
        }

        public AssignableMissingHandler(IEnumerable<Type> ignoreTypes)
        {
            this.ignoreTypes = new HashSet<Type>(ignoreTypes);
        }

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
