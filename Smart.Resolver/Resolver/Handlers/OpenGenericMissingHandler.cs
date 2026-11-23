namespace Smart.Resolver.Handlers;

using System.Diagnostics.CodeAnalysis;

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

    [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "The closed implementation type shares the constructors and properties of the open type definition, which the binding keeps.")]
    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "The closed implementation type shares the constructors and properties of the open type definition, which the binding keeps.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Reference type arguments share code. For value type arguments, the caller makes the closed type available, for example by using it in code.")]
    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        if (!type.IsGenericType || ignoreTypes.Contains(type))
        {
            return [];
        }

        var bindings = new List<Binding>();
        foreach (var b in table.FindBindings(type.GetGenericTypeDefinition()))
        {
            Type implementationType;
            try
            {
                implementationType = b.Provider.TargetType.MakeGenericType(type.GenericTypeArguments);
            }
            catch (ArgumentException)
            {
                continue;
            }

            bindings.Add(new Binding(
                type,
                new StandardProvider(implementationType, components),
                b.Scope?.Copy(components),
                b.Constraint,
                b.Metadata,
                b.ConstructorArguments,
                b.PropertyValues)
            {
                Order = b.Order
            });
        }

        return bindings;
    }
}
