namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Providers;

public sealed class OpenGenericMissingHandler : IMissingHandler
{
    private readonly HashSet<Type> ignoreTypes;

    public OpenGenericMissingHandler()
        : this(Type.EmptyTypes)
    {
    }

    public OpenGenericMissingHandler(IEnumerable<Type> ignoreTypes)
    {
#pragma warning disable IDE0055
        this.ignoreTypes = [..ignoreTypes];
#pragma warning restore IDE0055
    }

    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        if (!type.IsGenericType || ignoreTypes.Contains(type))
        {
            return [];
        }

        return table.FindBindings(type.GetGenericTypeDefinition())
            .Select(b => new Binding(
                type,
                new StandardProvider(b.Provider.TargetType.MakeGenericType(type.GenericTypeArguments), components),
                b.Scope?.Copy(components),
                b.Constraint,
                b.Metadata,
                b.ConstructorArguments,
                b.PropertyValues));
    }
}
