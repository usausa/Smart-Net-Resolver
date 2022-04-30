namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Providers;

public sealed class DependencyServiceMissingHandler : IMissingHandler
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
    public IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type)
    {
        if (!type.IsInterface)
        {
            return Enumerable.Empty<Binding>();
        }

        return new[]
        {
            new Binding(type, new DependencyServiceProvider(type))
        };
    }
}
