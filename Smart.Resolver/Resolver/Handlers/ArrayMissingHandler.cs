namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Helpers;
using Smart.Resolver.Providers;
using Smart.Resolver.Scopes;

public class ArrayMissingHandler : IMissingHandler
{
    private readonly HashSet<Type> ignoreElementTypes;

    public ArrayMissingHandler()
        : this(Type.EmptyTypes)
    {
    }

    public ArrayMissingHandler(IEnumerable<Type> ignoreElementTypes)
    {
        this.ignoreElementTypes = new HashSet<Type>(ignoreElementTypes);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Factory")]
    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        var elementType = TypeHelper.GetEnumerableElementType(type);
        if (elementType is null)
        {
            return Enumerable.Empty<Binding>();
        }

        if (ignoreElementTypes.Contains(elementType))
        {
            return Enumerable.Empty<Binding>();
        }

        var bindings = table.FindBindings(elementType);

        // hack for singleton
        var useSingleton = bindings.Length > 0 && bindings.All(static b => b.Scope is SingletonScope);
        return new[]
        {
            new Binding(
                type,
                new BindingArrayProvider(type, elementType, components, bindings),
                useSingleton ? new SingletonScope(components) : null,
                null,
                null,
                null)
        };
    }
}
