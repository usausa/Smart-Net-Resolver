namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IMissingHandler
    {
        IEnumerable<IBinding> Handle(IBindingTable table, Type type);
    }
}
