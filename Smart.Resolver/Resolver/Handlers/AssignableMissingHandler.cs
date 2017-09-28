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
    public class AssignableMissingHandler : IMissingHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <param name="table"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            return table.EnumBindings().Where(x => type.IsAssignableFrom(x.Provider.TargetType));
        }
    }
}
