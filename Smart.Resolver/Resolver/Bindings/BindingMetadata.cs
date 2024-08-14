namespace Smart.Resolver.Bindings;

using Smart.Collections.Generic;

public sealed class BindingMetadata
{
    private readonly Dictionary<string, object?>? values;

    public BindingMetadata()
        : this(null)
    {
    }

    public BindingMetadata(Dictionary<string, object?>? values)
    {
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
