namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Helpers;
using Smart.Resolver.Providers;
using Smart.Resolver.Scopes;

public sealed class ArrayMissingHandler : IMissingHandler
{
    private readonly HashSet<Type> ignoreElementTypes;

    public ArrayMissingHandler()
        : this(Type.EmptyTypes)
    {
    }

    public ArrayMissingHandler(IEnumerable<Type> ignoreElementTypes)
    {
        this.ignoreElementTypes = [..ignoreElementTypes];
    }

    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        var elementType = TypeHelper.GetEnumerableElementType(type);
        if (elementType is null)
        {
            return [];
        }

        if (ignoreElementTypes.Contains(elementType))
        {
            return [];
        }

        var bindings = table.FindBindings(elementType);

        // hack for singleton
        var useSingleton = bindings.Length > 0 && bindings.All(static b => b.Scope is SingletonScope);
#pragma warning disable CA2000
        return
        [
            new Binding(
                type,
                new BindingArrayProvider(type, elementType, components, bindings),
                useSingleton ? new SingletonScope(components) : null,
                null,
                null,
                null)
        ];
#pragma warning restore CA2000
    }
}
