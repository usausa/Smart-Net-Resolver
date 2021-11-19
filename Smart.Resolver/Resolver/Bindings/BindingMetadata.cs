namespace Smart.Resolver.Bindings;

using System.Collections.Generic;

using Smart.Collections.Generic;

public sealed class BindingMetadata
{
    private readonly IDictionary<string, object?>? values;

    public string? Name { get; }

    public BindingMetadata()
        : this(null, null)
    {
    }

    public BindingMetadata(string? name, IDictionary<string, object?>? values)
    {
        Name = name;
        this.values = values;
    }

    public bool Has(string key)
    {
        return (values is not null) && values.ContainsKey(key);
    }

    public T Get<T>(string key)
    {
        return Get<T>(key, default!);
    }

    public T Get<T>(string key, T defaultValue)
    {
        return values is null ? defaultValue! : (T)values.GetOr(key, defaultValue)!;
    }
}
