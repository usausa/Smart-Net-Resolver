namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    public interface IBindingTable
    {
        IBinding[] FindBindings(Type type);

        IEnumerable<IBinding> EnumBindings();
    }
}
