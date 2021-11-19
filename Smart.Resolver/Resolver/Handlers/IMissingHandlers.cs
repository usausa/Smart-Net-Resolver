namespace Smart.Resolver.Handlers;

using System;
using System.Collections.Generic;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;

public interface IMissingHandler
{
    IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type);
}
