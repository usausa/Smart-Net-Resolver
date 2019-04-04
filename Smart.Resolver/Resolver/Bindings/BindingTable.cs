namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class BindingTable : IBindingTable
    {
        private static readonly IBinding[] EmptyBindings = Array.Empty<IBinding>();

        private readonly Dictionary<Type, IBinding[]> table = new Dictionary<Type, IBinding[]>();

        public void Add(Type type, IBinding[] bindings)
        {
            table.Add(type, bindings);
        }

        public IBinding[] Get(Type type)
        {
            if (table.TryGetValue(type, out var bindings))
            {
                return bindings;
            }

            return null;
        }

        public IBinding[] FindBindings(Type type)
        {
            return table.TryGetValue(type, out var bindings) ? bindings : EmptyBindings;
        }

        public IEnumerable<IBinding> EnumBindings()
        {
            return table.SelectMany(x => x.Value);
        }
    }
}
