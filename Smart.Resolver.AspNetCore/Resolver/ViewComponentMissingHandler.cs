namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Mvc;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Providers;

    public class ViewComponentMissingHandler : IMissingHandler
    {
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if (type.GetCustomAttribute<ViewComponentAttribute>() != null)
            {
                return new[]
                {
                    new Binding(type, new StandardProvider(type, components))
                };
            }

            return Enumerable.Empty<IBinding>();
        }
    }
}
