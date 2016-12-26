namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IMissingHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <param name="table"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type);
    }
}
