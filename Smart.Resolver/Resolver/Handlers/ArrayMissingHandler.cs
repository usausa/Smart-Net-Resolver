namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Constraints;
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
#pragma warning disable IDE0055
        this.ignoreElementTypes = [..ignoreElementTypes];
#pragma warning restore IDE0055
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

        var closedBindings = table.FindBindings(elementType);
        IEnumerable<Binding> candidates = closedBindings;
        if (elementType.IsGenericType)
        {
            candidates = candidates.Concat(table.FindBindings(elementType.GetGenericTypeDefinition()));
        }

        var any = false;
        var useSingleton = true;
        var anyKeyed = false;
        var useSingletonKeyed = true;
        foreach (var candidate in candidates)
        {
            if (candidate.Constraint is null)
            {
                any = true;
                useSingleton &= candidate.Scope is SingletonScope;
            }
            else
            {
                anyKeyed = true;
                useSingletonKeyed &= candidate.Scope is SingletonScope;
            }
        }

        var provider = new BindingArrayProvider(type, elementType, components);
#pragma warning disable CA2000
        return
        [
            new Binding(
                type,
                provider,
                any && useSingleton ? new SingletonScope(components) : null,
                null,
                null,
                null,
                null),
            new Binding(
                type,
                provider,
                anyKeyed && useSingletonKeyed ? new SingletonScope(components) : null,
                MatchAnyConstraint.Instance,
                null,
                null,
                null)
        ];
#pragma warning restore CA2000
    }
}
