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
        this.ignoreTypes = new HashSet<Type>(ignoreTypes);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        if (!type.IsGenericType || ignoreTypes.Contains(type))
        {
            return Enumerable.Empty<Binding>();
        }

        return table.FindBindings(type.GetGenericTypeDefinition())
            .Select(b => new Binding(
                type,
                new StandardProvider(b.Provider.TargetType.MakeGenericType(type.GenericTypeArguments), components),
                b.Scope?.Copy(components),
                b.Metadata,
                b.ConstructorArguments,
                b.PropertyValues));
    }
}
