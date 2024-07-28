namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;

public sealed class AssignableMissingHandler : IMissingHandler
{
    private readonly HashSet<Type> targetTypes;

    public AssignableMissingHandler()
        : this(Type.EmptyTypes)
    {
    }

    public AssignableMissingHandler(IEnumerable<Type> types)
    {
#pragma warning disable IDE0055
        targetTypes = [..types];
#pragma warning restore IDE0055
    }

    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        if ((targetTypes.Count > 0) && !targetTypes.Contains(type))
        {
            return [];
        }

        return table.EnumBindings().Where(x => type.IsAssignableFrom(x.Provider.TargetType));
    }
}
