namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Providers;

public sealed class SelfMissingHandler : IMissingHandler
{
    private readonly HashSet<Type> ignoreTypes;

    public SelfMissingHandler()
        : this([typeof(string), typeof(Delegate)])
    {
    }

    public SelfMissingHandler(IEnumerable<Type> ignoreTypes)
    {
#pragma warning disable IDE0055
        this.ignoreTypes = [..ignoreTypes];
#pragma warning restore IDE0055
    }

    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        if (type.IsInterface || type.IsAbstract || type.IsValueType || type.ContainsGenericParameters ||
            ignoreTypes.Contains(type))
        {
            return [];
        }

        return
        [
            new Binding(type, new StandardProvider(type, components))
        ];
    }
}
