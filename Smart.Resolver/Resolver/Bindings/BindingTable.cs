namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class BindingTable
    {
        private static readonly Binding[] EmptyBindings = Array.Empty<Binding>();

        private readonly Dictionary<Type, Binding[]> table;

        public BindingTable(Dictionary<Type, Binding[]> table)
        {
            this.table = table;
        }

        public Binding[] Get(Type type)
        {
            if (table.TryGetValue(type, out var bindings))
            {
                return bindings;
            }

            return null;
        }

        public Binding[] FindBindings(Type type)
        {
            return table.TryGetValue(type, out var bindings) ? bindings : EmptyBindings;
        }

        public IEnumerable<Binding> EnumBindings()
        {
            return table.SelectMany(x => x.Value);
        }
    }
}
