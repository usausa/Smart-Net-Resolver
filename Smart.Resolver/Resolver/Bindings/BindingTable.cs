namespace Smart.Resolver.Bindings;

public sealed class BindingTable
{
    private static readonly Binding[] EmptyBindings = [];

    private readonly Dictionary<Type, Binding[]> table;

    public BindingTable(Dictionary<Type, Binding[]> table)
    {
        this.table = table;
    }

    public Binding[]? Get(Type type) => table.GetValueOrDefault(type);

    public Binding[] FindBindings(Type type) => table.GetValueOrDefault(type, EmptyBindings);

    public IEnumerable<Binding> EnumBindings() => table.SelectMany(static x => x.Value);
}
