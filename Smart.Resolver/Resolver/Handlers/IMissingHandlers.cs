namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public interface IMissingHandler
    {
        IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type);
    }
}
