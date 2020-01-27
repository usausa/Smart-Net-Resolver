namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public sealed class AssignableMissingHandler : IMissingHandler
    {
        private readonly HashSet<Type> targetTypes;

        public AssignableMissingHandler()
            : this(Type.EmptyTypes)
        {
        }

        public AssignableMissingHandler(IEnumerable<Type> types)
        {
            this.targetTypes = new HashSet<Type>(types);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if ((targetTypes.Count > 0) && !targetTypes.Contains(type))
            {
                return Enumerable.Empty<IBinding>();
            }

            return table.EnumBindings().Where(x => type.IsAssignableFrom(x.Provider.TargetType));
        }
    }
}
